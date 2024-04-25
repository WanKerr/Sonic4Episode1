using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

public partial class AppMain
{
    public static A2S_AMA_HEADER readAMAFile(string name)
    {
        using (Stream input = TitleContainer.OpenStream("Content\\" + name))
        {
            using (BinaryReader br = new BinaryReader(input))
                return readAMAFile(br);
        }
    }

    public static A2S_AMA_HEADER readAMAFile(object data)
    {
        if (data is A2S_AMA_HEADER)
            return (A2S_AMA_HEADER)data;
        AmbChunk ambChunk = (AmbChunk)data;
        return readAMAFile(ambChunk.array, ambChunk.offset);
    }

    public static A2S_AMA_HEADER readAMAFile(byte[] data, int offset)
    {
        using (MemoryStream memoryStream = new MemoryStream(data, offset, data.Length - offset))
        {
            using (BinaryReader br = new BinaryReader(memoryStream))
                return readAMAFile(br);
        }
    }

    public static A2S_AMA_HEADER readAMAFile(BinaryReader br)
    {
        A2S_AMA_HEADER a2SAmaHeader = new A2S_AMA_HEADER();
        br.BaseStream.Seek(4L, SeekOrigin.Current);
        a2SAmaHeader.version = br.ReadUInt32();
        a2SAmaHeader.node_num = br.ReadUInt32();
        a2SAmaHeader.act_num = br.ReadUInt32();
        a2SAmaHeader.node_tbl_offset = br.ReadInt32();
        a2SAmaHeader.act_tbl_offset = br.ReadInt32();
        a2SAmaHeader.node_name_tbl_offset = br.ReadInt32();
        a2SAmaHeader.act_name_tbl_offset = br.ReadInt32();
        a2SAmaHeader.node_tbl = new A2S_AMA_NODE[(int)a2SAmaHeader.node_num];
        a2SAmaHeader.act_tbl = new A2S_AMA_ACT[(int)a2SAmaHeader.act_num];
        if (a2SAmaHeader.node_tbl_offset != 0)
        {
            br.BaseStream.Seek(a2SAmaHeader.node_tbl_offset, SeekOrigin.Begin);
            for (int index = 0; index < a2SAmaHeader.node_num; ++index)
            {
                int key = br.ReadInt32();
                if (key != 0)
                {
                    if (!readAMAFile_nodeHash.ContainsKey(key))
                    {
                        a2SAmaHeader.node_tbl[index] = new A2S_AMA_NODE();
                        a2SAmaHeader.node_tbl[index]._off = key;
                        readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index]._off, a2SAmaHeader.node_tbl[index]);
                    }
                    else
                        a2SAmaHeader.node_tbl[index] = readAMAFile_nodeHash[key];
                }
            }
            for (int index = 0; index < a2SAmaHeader.node_num; ++index)
            {
                br.BaseStream.Seek(a2SAmaHeader.node_tbl[index]._off, SeekOrigin.Begin);
                a2SAmaHeader.node_tbl[index].flag = br.ReadUInt32();
                a2SAmaHeader.node_tbl[index].id = br.ReadUInt32();
                a2SAmaHeader.node_tbl[index].child_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].child_offset != 0)
                {
                    if (!readAMAFile_nodeHash.ContainsKey(a2SAmaHeader.node_tbl[index].child_offset))
                    {
                        a2SAmaHeader.node_tbl[index].child = new A2S_AMA_NODE();
                        readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index].child_offset, a2SAmaHeader.node_tbl[index].child);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].child = readAMAFile_nodeHash[a2SAmaHeader.node_tbl[index].child_offset];
                }
                a2SAmaHeader.node_tbl[index].sibling_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].sibling_offset != 0)
                {
                    if (!readAMAFile_nodeHash.ContainsKey(a2SAmaHeader.node_tbl[index].sibling_offset))
                    {
                        a2SAmaHeader.node_tbl[index].sibling = new A2S_AMA_NODE();
                        readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index].sibling_offset, a2SAmaHeader.node_tbl[index].sibling);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].sibling = readAMAFile_nodeHash[a2SAmaHeader.node_tbl[index].sibling_offset];
                }
                a2SAmaHeader.node_tbl[index].parent_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].parent_offset != 0)
                {
                    if (!readAMAFile_nodeHash.ContainsKey(a2SAmaHeader.node_tbl[index].parent_offset))
                    {
                        a2SAmaHeader.node_tbl[index].parent = new A2S_AMA_NODE();
                        readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index].parent_offset, a2SAmaHeader.node_tbl[index].parent);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].parent = readAMAFile_nodeHash[a2SAmaHeader.node_tbl[index].parent_offset];
                }
                a2SAmaHeader.node_tbl[index].act_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].act_offset != 0)
                {
                    if (!readAMAFile_actHash.ContainsKey(a2SAmaHeader.node_tbl[index].act_offset))
                    {
                        a2SAmaHeader.node_tbl[index].act = new A2S_AMA_ACT();
                        a2SAmaHeader.node_tbl[index].act._off = a2SAmaHeader.node_tbl[index].act_offset;
                        readAMAFile_actHash.Add(a2SAmaHeader.node_tbl[index].act_offset, a2SAmaHeader.node_tbl[index].act);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].act = readAMAFile_actHash[a2SAmaHeader.node_tbl[index].act_offset];
                }
                br.BaseStream.Seek(8L, SeekOrigin.Current);
            }
            br.BaseStream.Seek(a2SAmaHeader.node_name_tbl_offset, SeekOrigin.Begin);
            int[] numArray = new int[(int)a2SAmaHeader.node_num];
            for (int index = 0; index < a2SAmaHeader.node_num; ++index)
                numArray[index] = br.ReadInt32();
            for (int index = 0; index < a2SAmaHeader.node_num; ++index)
            {
                br.BaseStream.Seek(numArray[index], SeekOrigin.Begin);
                skipString(br);
            }
        }
        if (a2SAmaHeader.act_tbl_offset != 0)
        {
            br.BaseStream.Seek(a2SAmaHeader.act_tbl_offset, SeekOrigin.Begin);
            for (int index = 0; index < a2SAmaHeader.act_num; ++index)
            {
                int key = br.ReadInt32();
                if (!readAMAFile_actHash.ContainsKey(key))
                {
                    a2SAmaHeader.act_tbl[index] = new A2S_AMA_ACT();
                    a2SAmaHeader.act_tbl[index]._off = key;
                    readAMAFile_actHash.Add(a2SAmaHeader.act_tbl[index]._off, a2SAmaHeader.act_tbl[index]);
                }
                else
                    a2SAmaHeader.act_tbl[index] = readAMAFile_actHash[key];
            }
            for (int index = 0; index < a2SAmaHeader.act_num; ++index)
            {
                br.BaseStream.Seek(a2SAmaHeader.act_tbl[index]._off, SeekOrigin.Begin);
                a2SAmaHeader.act_tbl[index].flag = br.ReadUInt32();
                a2SAmaHeader.act_tbl[index].id = br.ReadUInt32();
                a2SAmaHeader.act_tbl[index].frm_num = br.ReadUInt32();
                a2SAmaHeader.act_tbl[index].pad1 = br.ReadUInt32();
                a2SAmaHeader.act_tbl[index].ofst.left = br.ReadSingle();
                a2SAmaHeader.act_tbl[index].ofst.top = br.ReadSingle();
                a2SAmaHeader.act_tbl[index].ofst.right = br.ReadSingle();
                a2SAmaHeader.act_tbl[index].ofst.bottom = br.ReadSingle();
                a2SAmaHeader.act_tbl[index].mtn_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].mtn_offset != 0)
                {
                    if (!readAMAFile_mtnHash.ContainsKey(a2SAmaHeader.act_tbl[index].mtn_offset))
                    {
                        a2SAmaHeader.act_tbl[index].mtn = new A2S_AMA_MTN();
                        readAMAFile_mtnHash.Add(a2SAmaHeader.act_tbl[index].mtn_offset, a2SAmaHeader.act_tbl[index].mtn);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].mtn = readAMAFile_mtnHash[a2SAmaHeader.act_tbl[index].mtn_offset];
                }
                a2SAmaHeader.act_tbl[index].anm_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].anm_offset != 0)
                {
                    if (!readAMAFile_anmHash.ContainsKey(a2SAmaHeader.act_tbl[index].anm_offset))
                    {
                        a2SAmaHeader.act_tbl[index].anm = new A2S_AMA_ANM();
                        readAMAFile_anmHash.Add(a2SAmaHeader.act_tbl[index].anm_offset, a2SAmaHeader.act_tbl[index].anm);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].anm = readAMAFile_anmHash[a2SAmaHeader.act_tbl[index].anm_offset];
                }
                a2SAmaHeader.act_tbl[index].acm_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].acm_offset != 0)
                {
                    if (!readAMAFile_acmHash.ContainsKey(a2SAmaHeader.act_tbl[index].acm_offset))
                    {
                        a2SAmaHeader.act_tbl[index].acm = new A2S_AMA_ACM();
                        readAMAFile_acmHash.Add(a2SAmaHeader.act_tbl[index].acm_offset, a2SAmaHeader.act_tbl[index].acm);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].acm = readAMAFile_acmHash[a2SAmaHeader.act_tbl[index].acm_offset];
                }
                a2SAmaHeader.act_tbl[index].usr_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].usr_offset != 0)
                {
                    if (!readAMAFile_usrHash.ContainsKey(a2SAmaHeader.act_tbl[index].usr_offset))
                    {
                        a2SAmaHeader.act_tbl[index].usr = new A2S_AMA_USR();
                        readAMAFile_usrHash.Add(a2SAmaHeader.act_tbl[index].usr_offset, a2SAmaHeader.act_tbl[index].usr);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].usr = readAMAFile_usrHash[a2SAmaHeader.act_tbl[index].usr_offset];
                }
                a2SAmaHeader.act_tbl[index].hit_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].hit_offset != 0)
                {
                    if (!readAMAFile_hitHash.ContainsKey(a2SAmaHeader.act_tbl[index].hit_offset))
                    {
                        a2SAmaHeader.act_tbl[index].hit = new A2S_AMA_HIT();
                        readAMAFile_hitHash.Add(a2SAmaHeader.act_tbl[index].hit_offset, a2SAmaHeader.act_tbl[index].hit);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].hit = readAMAFile_hitHash[a2SAmaHeader.act_tbl[index].hit_offset];
                }
                a2SAmaHeader.act_tbl[index].next_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].next_offset != 0)
                {
                    if (!readAMAFile_actHash.ContainsKey(a2SAmaHeader.act_tbl[index].next_offset))
                    {
                        a2SAmaHeader.act_tbl[index].next = new A2S_AMA_ACT();
                        readAMAFile_actHash.Add(a2SAmaHeader.act_tbl[index].next_offset, a2SAmaHeader.act_tbl[index].next);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].next = readAMAFile_actHash[a2SAmaHeader.act_tbl[index].next_offset];
                }
                br.BaseStream.Seek(8L, SeekOrigin.Current);
            }
            br.BaseStream.Seek(a2SAmaHeader.act_name_tbl_offset, SeekOrigin.Begin);
            int[] numArray = new int[(int)a2SAmaHeader.act_num];
            for (int index = 0; index < a2SAmaHeader.act_num; ++index)
                numArray[index] = br.ReadInt32();
            for (int index = 0; index < a2SAmaHeader.act_num; ++index)
            {
                br.BaseStream.Seek(numArray[index], SeekOrigin.Begin);
                skipString(br);
            }
            foreach (KeyValuePair<int, A2S_AMA_MTN> keyValuePair in readAMAFile_mtnHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_AMA_MTN a2SAmaMtn = keyValuePair.Value;
                a2SAmaMtn.flag = br.ReadUInt32();
                a2SAmaMtn.mtn_key_num = br.ReadUInt32();
                a2SAmaMtn.mtn_frm_num = br.ReadUInt32();
                a2SAmaMtn.mtn_key_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.mtn_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaMtn.mtn_key_tbl_offset))
                    {
                        a2SAmaMtn.mtn_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaMtn.mtn_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaMtn.mtn_key_tbl_offset, a2SAmaMtn.mtn_key_tbl);
                    }
                    else
                        a2SAmaMtn.mtn_key_tbl = readAMAFile_subkeyHash[a2SAmaMtn.mtn_key_tbl_offset];
                }
                a2SAmaMtn.mtn_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.mtn_tbl_offset != 0)
                {
                    if (!readAMAFile_submtnHash.ContainsKey(a2SAmaMtn.mtn_tbl_offset))
                    {
                        a2SAmaMtn.mtn_tbl = new A2S_SUB_MTN[(int)(a2SAmaMtn.mtn_key_num + 1U)];
                        readAMAFile_submtnHash.Add(a2SAmaMtn.mtn_tbl_offset, a2SAmaMtn.mtn_tbl);
                    }
                    else
                        a2SAmaMtn.mtn_tbl = readAMAFile_submtnHash[a2SAmaMtn.mtn_tbl_offset];
                }
                a2SAmaMtn.trs_key_num = br.ReadUInt32();
                a2SAmaMtn.trs_frm_num = br.ReadUInt32();
                a2SAmaMtn.trs_key_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.trs_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaMtn.trs_key_tbl_offset))
                    {
                        a2SAmaMtn.trs_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaMtn.trs_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaMtn.trs_key_tbl_offset, a2SAmaMtn.trs_key_tbl);
                    }
                    else
                        a2SAmaMtn.trs_key_tbl = readAMAFile_subkeyHash[a2SAmaMtn.trs_key_tbl_offset];
                }
                a2SAmaMtn.trs_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.trs_tbl_offset != 0)
                {
                    if (!readAMAFile_subtrsHash.ContainsKey(a2SAmaMtn.trs_tbl_offset))
                    {
                        a2SAmaMtn.trs_tbl = new A2S_SUB_TRS[(int)(a2SAmaMtn.trs_key_num + 1U)];
                        readAMAFile_subtrsHash.Add(a2SAmaMtn.trs_tbl_offset, a2SAmaMtn.trs_tbl);
                    }
                    else
                        a2SAmaMtn.trs_tbl = readAMAFile_subtrsHash[a2SAmaMtn.trs_tbl_offset];
                }
            }
            foreach (KeyValuePair<int, A2S_AMA_ANM> keyValuePair in readAMAFile_anmHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_AMA_ANM a2SAmaAnm = keyValuePair.Value;
                a2SAmaAnm.flag = br.ReadUInt32();
                a2SAmaAnm.anm_key_num = br.ReadUInt32();
                a2SAmaAnm.anm_frm_num = br.ReadUInt32();
                a2SAmaAnm.anm_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.anm_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaAnm.anm_key_tbl_offset))
                    {
                        a2SAmaAnm.anm_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaAnm.anm_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaAnm.anm_key_tbl_offset, a2SAmaAnm.anm_key_tbl);
                    }
                    else
                        a2SAmaAnm.anm_key_tbl = readAMAFile_subkeyHash[a2SAmaAnm.anm_key_tbl_offset];
                }
                a2SAmaAnm.anm_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.anm_tbl_offset != 0)
                {
                    if (!readAMAFile_subanmHash.ContainsKey(a2SAmaAnm.anm_tbl_offset))
                    {
                        a2SAmaAnm.anm_tbl = new A2S_SUB_ANM[(int)(a2SAmaAnm.anm_key_num + 1U)];
                        readAMAFile_subanmHash.Add(a2SAmaAnm.anm_tbl_offset, a2SAmaAnm.anm_tbl);
                    }
                    else
                        a2SAmaAnm.anm_tbl = readAMAFile_subanmHash[a2SAmaAnm.anm_tbl_offset];
                }
                a2SAmaAnm.mat_key_num = br.ReadUInt32();
                a2SAmaAnm.mat_frm_num = br.ReadUInt32();
                a2SAmaAnm.mat_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.mat_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaAnm.mat_key_tbl_offset))
                    {
                        a2SAmaAnm.mat_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaAnm.mat_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaAnm.mat_key_tbl_offset, a2SAmaAnm.mat_key_tbl);
                    }
                    else
                        a2SAmaAnm.mat_key_tbl = readAMAFile_subkeyHash[a2SAmaAnm.mat_key_tbl_offset];
                }
                a2SAmaAnm.mat_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.mat_tbl_offset != 0)
                {
                    if (!readAMAFile_submatHash.ContainsKey(a2SAmaAnm.mat_tbl_offset))
                    {
                        a2SAmaAnm.mat_tbl = new A2S_SUB_MAT[(int)(a2SAmaAnm.mat_key_num + 1U)];
                        readAMAFile_submatHash.Add(a2SAmaAnm.mat_tbl_offset, a2SAmaAnm.mat_tbl);
                    }
                    else
                        a2SAmaAnm.mat_tbl = readAMAFile_submatHash[a2SAmaAnm.mat_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, A2S_AMA_ACM> keyValuePair in readAMAFile_acmHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_AMA_ACM a2SAmaAcm = keyValuePair.Value;
                a2SAmaAcm.flag = br.ReadUInt32();
                a2SAmaAcm.acm_key_num = br.ReadUInt32();
                a2SAmaAcm.acm_frm_num = br.ReadUInt32();
                a2SAmaAcm.acm_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.acm_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaAcm.acm_key_tbl_offset))
                    {
                        a2SAmaAcm.acm_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaAcm.acm_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaAcm.acm_key_tbl_offset, a2SAmaAcm.acm_key_tbl);
                    }
                    else
                        a2SAmaAcm.acm_key_tbl = readAMAFile_subkeyHash[a2SAmaAcm.acm_key_tbl_offset];
                }
                a2SAmaAcm.acm_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.acm_tbl_offset != 0)
                {
                    if (!readAMAFile_subacmHash.ContainsKey(a2SAmaAcm.acm_tbl_offset))
                    {
                        a2SAmaAcm.acm_tbl = new A2S_SUB_ACM[(int)(a2SAmaAcm.acm_key_num + 1U)];
                        readAMAFile_subacmHash.Add(a2SAmaAcm.acm_tbl_offset, a2SAmaAcm.acm_tbl);
                    }
                    else
                        a2SAmaAcm.acm_tbl = readAMAFile_subacmHash[a2SAmaAcm.acm_tbl_offset];
                }
                a2SAmaAcm.trs_key_num = br.ReadUInt32();
                a2SAmaAcm.trs_frm_num = br.ReadUInt32();
                a2SAmaAcm.trs_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.trs_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaAcm.trs_key_tbl_offset))
                    {
                        a2SAmaAcm.trs_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaAcm.trs_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaAcm.trs_key_tbl_offset, a2SAmaAcm.trs_key_tbl);
                    }
                    else
                        a2SAmaAcm.trs_key_tbl = readAMAFile_subkeyHash[a2SAmaAcm.trs_key_tbl_offset];
                }
                a2SAmaAcm.trs_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.trs_tbl_offset != 0)
                {
                    if (!readAMAFile_subtrsHash.ContainsKey(a2SAmaAcm.trs_tbl_offset))
                    {
                        a2SAmaAcm.trs_tbl = new A2S_SUB_TRS[(int)(a2SAmaAcm.trs_key_num + 1U)];
                        readAMAFile_subtrsHash.Add(a2SAmaAcm.trs_tbl_offset, a2SAmaAcm.trs_tbl);
                    }
                    else
                        a2SAmaAcm.trs_tbl = readAMAFile_subtrsHash[a2SAmaAcm.trs_tbl_offset];
                }
                a2SAmaAcm.mat_key_num = br.ReadUInt32();
                a2SAmaAcm.mat_frm_num = br.ReadUInt32();
                a2SAmaAcm.mat_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.mat_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaAcm.mat_key_tbl_offset))
                    {
                        a2SAmaAcm.mat_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaAcm.mat_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaAcm.mat_key_tbl_offset, a2SAmaAcm.mat_key_tbl);
                    }
                    else
                        a2SAmaAcm.mat_key_tbl = readAMAFile_subkeyHash[a2SAmaAcm.mat_key_tbl_offset];
                }
                a2SAmaAcm.mat_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.mat_tbl_offset != 0)
                {
                    if (!readAMAFile_submatHash.ContainsKey(a2SAmaAcm.mat_tbl_offset))
                    {
                        a2SAmaAcm.mat_tbl = new A2S_SUB_MAT[(int)(a2SAmaAcm.mat_key_num + 1U)];
                        readAMAFile_submatHash.Add(a2SAmaAcm.mat_tbl_offset, a2SAmaAcm.mat_tbl);
                    }
                    else
                        a2SAmaAcm.mat_tbl = readAMAFile_submatHash[a2SAmaAcm.mat_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, A2S_AMA_USR> keyValuePair in readAMAFile_usrHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_AMA_USR a2SAmaUsr = keyValuePair.Value;
                a2SAmaUsr.flag = br.ReadUInt32();
                a2SAmaUsr.usr_key_num = br.ReadUInt32();
                a2SAmaUsr.usr_frm_num = br.ReadUInt32();
                a2SAmaUsr.usr_key_tbl_offset = br.ReadInt32();
                if (a2SAmaUsr.usr_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaUsr.usr_key_tbl_offset))
                    {
                        a2SAmaUsr.usr_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaUsr.usr_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaUsr.usr_key_tbl_offset, a2SAmaUsr.usr_key_tbl);
                    }
                    else
                        a2SAmaUsr.usr_key_tbl = readAMAFile_subkeyHash[a2SAmaUsr.usr_key_tbl_offset];
                }
                a2SAmaUsr.usr_tbl_offset = br.ReadInt32();
                if (a2SAmaUsr.usr_tbl_offset != 0)
                {
                    if (!readAMAFile_subusrHash.ContainsKey(a2SAmaUsr.usr_tbl_offset))
                    {
                        a2SAmaUsr.usr_tbl = new A2S_SUB_USR[(int)(a2SAmaUsr.usr_key_num + 1U)];
                        readAMAFile_subusrHash.Add(a2SAmaUsr.usr_tbl_offset, a2SAmaUsr.usr_tbl);
                    }
                    else
                        a2SAmaUsr.usr_tbl = readAMAFile_subusrHash[a2SAmaUsr.usr_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, A2S_AMA_HIT> keyValuePair in readAMAFile_hitHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_AMA_HIT a2SAmaHit = keyValuePair.Value;
                a2SAmaHit.flag = br.ReadUInt32();
                a2SAmaHit.hit_key_num = br.ReadUInt32();
                a2SAmaHit.hit_frm_num = br.ReadUInt32();
                a2SAmaHit.hit_key_tbl_offset = br.ReadInt32();
                if (a2SAmaHit.hit_key_tbl_offset != 0)
                {
                    if (!readAMAFile_subkeyHash.ContainsKey(a2SAmaHit.hit_key_tbl_offset))
                    {
                        a2SAmaHit.hit_key_tbl = new A2S_SUB_KEY[(int)(a2SAmaHit.hit_key_num + 1U)];
                        readAMAFile_subkeyHash.Add(a2SAmaHit.hit_key_tbl_offset, a2SAmaHit.hit_key_tbl);
                    }
                    else
                        a2SAmaHit.hit_key_tbl = readAMAFile_subkeyHash[a2SAmaHit.hit_key_tbl_offset];
                }
                a2SAmaHit.hit_tbl_offset = br.ReadInt32();
                if (a2SAmaHit.hit_tbl_offset != 0)
                {
                    if (!readAMAFile_subhitHash.ContainsKey(a2SAmaHit.hit_tbl_offset))
                    {
                        a2SAmaHit.hit_tbl = new A2S_SUB_HIT[(int)(a2SAmaHit.hit_key_num + 1U)];
                        readAMAFile_subhitHash.Add(a2SAmaHit.hit_tbl_offset, a2SAmaHit.hit_tbl);
                    }
                    else
                        a2SAmaHit.hit_tbl = readAMAFile_subhitHash[a2SAmaHit.hit_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, A2S_SUB_TRS[]> keyValuePair in readAMAFile_subtrsHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_SUB_TRS[] a2SSubTrsArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubTrsArray[index] = new A2S_SUB_TRS();
                    a2SSubTrsArray[index].trs_x = br.ReadSingle();
                    a2SSubTrsArray[index].trs_y = br.ReadSingle();
                    a2SSubTrsArray[index].trs_z = br.ReadSingle();
                    a2SSubTrsArray[index].trs_accele = br.ReadSingle();
                }
            }
            foreach (KeyValuePair<int, A2S_SUB_MTN[]> keyValuePair in readAMAFile_submtnHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_SUB_MTN[] a2SSubMtnArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubMtnArray[index] = new A2S_SUB_MTN();
                    a2SSubMtnArray[index].scl_x = br.ReadSingle();
                    a2SSubMtnArray[index].scl_y = br.ReadSingle();
                    a2SSubMtnArray[index].rot = br.ReadSingle();
                    a2SSubMtnArray[index].scl_accele = br.ReadSingle();
                    a2SSubMtnArray[index].rot_accele = br.ReadSingle();
                    br.BaseStream.Seek(12L, SeekOrigin.Current);
                }
            }
            try
            {
                foreach (KeyValuePair<int, A2S_SUB_ANM[]> keyValuePair in readAMAFile_subanmHash)
                {
                    br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                    A2S_SUB_ANM[] a2SSubAnmArray = keyValuePair.Value;
                    int length = keyValuePair.Value.Length;
                    for (int index = 0; index < length; ++index)
                    {
                        a2SSubAnmArray[index] = new A2S_SUB_ANM();
                        a2SSubAnmArray[index].tex_id = br.ReadInt32();
                        a2SSubAnmArray[index].clamp = br.ReadUInt32();
                        a2SSubAnmArray[index].filter = br.ReadUInt32();
                        a2SSubAnmArray[index].texel_accele = br.ReadSingle();
                        a2SSubAnmArray[index].texel.left = br.ReadSingle();
                        a2SSubAnmArray[index].texel.top = br.ReadSingle();
                        a2SSubAnmArray[index].texel.right = br.ReadSingle();
                        a2SSubAnmArray[index].texel.bottom = br.ReadSingle();
                    }
                }
            }
            catch (EndOfStreamException ex)
            {
            }
            foreach (KeyValuePair<int, A2S_SUB_MAT[]> keyValuePair in readAMAFile_submatHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_SUB_MAT[] a2SSubMatArray = keyValuePair.Value;
                int num = keyValuePair.Value.Length - 1;
                for (int index = 0; index < num; ++index)
                {
                    a2SSubMatArray[index] = new A2S_SUB_MAT();
                    a2SSubMatArray[index].base_.a = br.ReadByte();
                    a2SSubMatArray[index].base_.b = br.ReadByte();
                    a2SSubMatArray[index].base_.g = br.ReadByte();
                    a2SSubMatArray[index].base_.r = br.ReadByte();
                    a2SSubMatArray[index].fade.a = br.ReadByte();
                    a2SSubMatArray[index].fade.b = br.ReadByte();
                    a2SSubMatArray[index].fade.g = br.ReadByte();
                    a2SSubMatArray[index].fade.r = br.ReadByte();
                    a2SSubMatArray[index].base_accele = br.ReadSingle();
                    a2SSubMatArray[index].fade_accele = br.ReadSingle();
                    a2SSubMatArray[index].blend = br.ReadUInt32();
                    br.BaseStream.Seek(12L, SeekOrigin.Current);
                }
            }
            foreach (KeyValuePair<int, A2S_SUB_ACM[]> keyValuePair in readAMAFile_subacmHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_SUB_ACM[] a2SSubAcmArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubAcmArray[index] = new A2S_SUB_ACM();
                    a2SSubAcmArray[index].trs_scl_x = br.ReadSingle();
                    a2SSubAcmArray[index].trs_scl_y = br.ReadSingle();
                    a2SSubAcmArray[index].scl_x = br.ReadSingle();
                    a2SSubAcmArray[index].scl_y = br.ReadSingle();
                    a2SSubAcmArray[index].rot = br.ReadSingle();
                    a2SSubAcmArray[index].trs_scl_accele = br.ReadSingle();
                    a2SSubAcmArray[index].scl_accele = br.ReadSingle();
                    a2SSubAcmArray[index].rot_accele = br.ReadSingle();
                }
            }
            foreach (KeyValuePair<int, A2S_SUB_USR[]> keyValuePair in readAMAFile_subusrHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_SUB_USR[] a2SSubUsrArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubUsrArray[index].usr_id = br.ReadUInt32();
                    br.BaseStream.Seek(12L, SeekOrigin.Current);
                    a2SSubUsrArray[index].usr_accele = br.ReadSingle();
                    br.BaseStream.Seek(12L, SeekOrigin.Current);
                }
            }
            foreach (KeyValuePair<int, A2S_SUB_HIT[]> keyValuePair in readAMAFile_subhitHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_SUB_HIT[] a2SSubHitArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubHitArray[index] = new A2S_SUB_HIT();
                    a2SSubHitArray[index].flag = br.ReadUInt32();
                    a2SSubHitArray[index].type = br.ReadUInt32();
                    a2SSubHitArray[index].hit_accele = br.ReadSingle();
                    a2SSubHitArray[index].pad = br.ReadUInt32();
                    a2SSubHitArray[index].rect.left = br.ReadSingle();
                    a2SSubHitArray[index].rect.top = br.ReadSingle();
                    a2SSubHitArray[index].rect.right = br.ReadSingle();
                    a2SSubHitArray[index].rect.bottom = br.ReadSingle();
                    a2SSubHitArray[index].circle.center_x = a2SSubHitArray[index].rect.left;
                    a2SSubHitArray[index].circle.radius = a2SSubHitArray[index].rect.right;
                    a2SSubHitArray[index].circle.pad = (uint)a2SSubHitArray[index].rect.bottom;
                }
            }
            foreach (KeyValuePair<int, A2S_SUB_KEY[]> keyValuePair in readAMAFile_subkeyHash)
            {
                br.BaseStream.Seek(keyValuePair.Key, SeekOrigin.Begin);
                A2S_SUB_KEY[] a2SSubKeyArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubKeyArray[index] = new A2S_SUB_KEY();
                    a2SSubKeyArray[index].frm = br.ReadUInt32();
                    a2SSubKeyArray[index].interpol = br.ReadUInt32();
                }
            }
        }
        readAMAFile_nodeHash.Clear();
        readAMAFile_actHash.Clear();
        readAMAFile_mtnHash.Clear();
        readAMAFile_anmHash.Clear();
        readAMAFile_acmHash.Clear();
        readAMAFile_usrHash.Clear();
        readAMAFile_hitHash.Clear();
        readAMAFile_subtrsHash.Clear();
        readAMAFile_submtnHash.Clear();
        readAMAFile_subanmHash.Clear();
        readAMAFile_submatHash.Clear();
        readAMAFile_subacmHash.Clear();
        readAMAFile_subusrHash.Clear();
        readAMAFile_subhitHash.Clear();
        readAMAFile_subkeyHash.Clear();
        return a2SAmaHeader;
    }

    private static void skipString(BinaryReader br)
    {
        do
            ;
        while (br.ReadChar() != char.MinValue);
    }

    private static string readChars(BinaryReader br)
    {
        int length = 0;
        while (true)
        {
            char ch = br.ReadChar();
            if (ch != char.MinValue)
            {
                readChars_name[length] = ch;
                ++length;
            }
            else
                break;
        }
        return new string(readChars_name, 0, length);
    }

}