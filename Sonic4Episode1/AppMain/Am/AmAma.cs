using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    public static AppMain.A2S_AMA_HEADER readAMAFile(string name)
    {
        using (Stream input = TitleContainer.OpenStream("Content\\" + name))
        {
            using (BinaryReader br = new BinaryReader(input))
                return AppMain.readAMAFile(br);
        }
    }

    public static AppMain.A2S_AMA_HEADER readAMAFile(object data)
    {
        if (data is AppMain.A2S_AMA_HEADER)
            return (AppMain.A2S_AMA_HEADER)data;
        AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)data;
        return AppMain.readAMAFile(ambChunk.array, ambChunk.offset);
    }

    public static AppMain.A2S_AMA_HEADER readAMAFile(byte[] data, int offset)
    {
        using (MemoryStream memoryStream = new MemoryStream(data, offset, data.Length - offset))
        {
            using (BinaryReader br = new BinaryReader((Stream)memoryStream))
                return AppMain.readAMAFile(br);
        }
    }

    public static AppMain.A2S_AMA_HEADER readAMAFile(BinaryReader br)
    {
        AppMain.A2S_AMA_HEADER a2SAmaHeader = new AppMain.A2S_AMA_HEADER();
        br.BaseStream.Seek(4L, SeekOrigin.Current);
        a2SAmaHeader.version = br.ReadUInt32();
        a2SAmaHeader.node_num = br.ReadUInt32();
        a2SAmaHeader.act_num = br.ReadUInt32();
        a2SAmaHeader.node_tbl_offset = br.ReadInt32();
        a2SAmaHeader.act_tbl_offset = br.ReadInt32();
        a2SAmaHeader.node_name_tbl_offset = br.ReadInt32();
        a2SAmaHeader.act_name_tbl_offset = br.ReadInt32();
        a2SAmaHeader.node_tbl = new AppMain.A2S_AMA_NODE[(int)a2SAmaHeader.node_num];
        a2SAmaHeader.act_tbl = new AppMain.A2S_AMA_ACT[(int)a2SAmaHeader.act_num];
        if (a2SAmaHeader.node_tbl_offset != 0)
        {
            br.BaseStream.Seek((long)a2SAmaHeader.node_tbl_offset, SeekOrigin.Begin);
            for (int index = 0; (long)index < (long)a2SAmaHeader.node_num; ++index)
            {
                int key = br.ReadInt32();
                if (key != 0)
                {
                    if (!AppMain.readAMAFile_nodeHash.ContainsKey(key))
                    {
                        a2SAmaHeader.node_tbl[index] = new AppMain.A2S_AMA_NODE();
                        a2SAmaHeader.node_tbl[index]._off = key;
                        AppMain.readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index]._off, a2SAmaHeader.node_tbl[index]);
                    }
                    else
                        a2SAmaHeader.node_tbl[index] = AppMain.readAMAFile_nodeHash[key];
                }
            }
            for (int index = 0; (long)index < (long)a2SAmaHeader.node_num; ++index)
            {
                br.BaseStream.Seek((long)a2SAmaHeader.node_tbl[index]._off, SeekOrigin.Begin);
                a2SAmaHeader.node_tbl[index].flag = br.ReadUInt32();
                a2SAmaHeader.node_tbl[index].id = br.ReadUInt32();
                a2SAmaHeader.node_tbl[index].child_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].child_offset != 0)
                {
                    if (!AppMain.readAMAFile_nodeHash.ContainsKey(a2SAmaHeader.node_tbl[index].child_offset))
                    {
                        a2SAmaHeader.node_tbl[index].child = new AppMain.A2S_AMA_NODE();
                        AppMain.readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index].child_offset, a2SAmaHeader.node_tbl[index].child);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].child = AppMain.readAMAFile_nodeHash[a2SAmaHeader.node_tbl[index].child_offset];
                }
                a2SAmaHeader.node_tbl[index].sibling_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].sibling_offset != 0)
                {
                    if (!AppMain.readAMAFile_nodeHash.ContainsKey(a2SAmaHeader.node_tbl[index].sibling_offset))
                    {
                        a2SAmaHeader.node_tbl[index].sibling = new AppMain.A2S_AMA_NODE();
                        AppMain.readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index].sibling_offset, a2SAmaHeader.node_tbl[index].sibling);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].sibling = AppMain.readAMAFile_nodeHash[a2SAmaHeader.node_tbl[index].sibling_offset];
                }
                a2SAmaHeader.node_tbl[index].parent_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].parent_offset != 0)
                {
                    if (!AppMain.readAMAFile_nodeHash.ContainsKey(a2SAmaHeader.node_tbl[index].parent_offset))
                    {
                        a2SAmaHeader.node_tbl[index].parent = new AppMain.A2S_AMA_NODE();
                        AppMain.readAMAFile_nodeHash.Add(a2SAmaHeader.node_tbl[index].parent_offset, a2SAmaHeader.node_tbl[index].parent);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].parent = AppMain.readAMAFile_nodeHash[a2SAmaHeader.node_tbl[index].parent_offset];
                }
                a2SAmaHeader.node_tbl[index].act_offset = br.ReadInt32();
                if (a2SAmaHeader.node_tbl[index].act_offset != 0)
                {
                    if (!AppMain.readAMAFile_actHash.ContainsKey(a2SAmaHeader.node_tbl[index].act_offset))
                    {
                        a2SAmaHeader.node_tbl[index].act = new AppMain.A2S_AMA_ACT();
                        a2SAmaHeader.node_tbl[index].act._off = a2SAmaHeader.node_tbl[index].act_offset;
                        AppMain.readAMAFile_actHash.Add(a2SAmaHeader.node_tbl[index].act_offset, a2SAmaHeader.node_tbl[index].act);
                    }
                    else
                        a2SAmaHeader.node_tbl[index].act = AppMain.readAMAFile_actHash[a2SAmaHeader.node_tbl[index].act_offset];
                }
                br.BaseStream.Seek(8L, SeekOrigin.Current);
            }
            br.BaseStream.Seek((long)a2SAmaHeader.node_name_tbl_offset, SeekOrigin.Begin);
            int[] numArray = new int[(int)a2SAmaHeader.node_num];
            for (int index = 0; (long)index < (long)a2SAmaHeader.node_num; ++index)
                numArray[index] = br.ReadInt32();
            for (int index = 0; (long)index < (long)a2SAmaHeader.node_num; ++index)
            {
                br.BaseStream.Seek((long)numArray[index], SeekOrigin.Begin);
                AppMain.skipString(br);
            }
        }
        if (a2SAmaHeader.act_tbl_offset != 0)
        {
            br.BaseStream.Seek((long)a2SAmaHeader.act_tbl_offset, SeekOrigin.Begin);
            for (int index = 0; (long)index < (long)a2SAmaHeader.act_num; ++index)
            {
                int key = br.ReadInt32();
                if (!AppMain.readAMAFile_actHash.ContainsKey(key))
                {
                    a2SAmaHeader.act_tbl[index] = new AppMain.A2S_AMA_ACT();
                    a2SAmaHeader.act_tbl[index]._off = key;
                    AppMain.readAMAFile_actHash.Add(a2SAmaHeader.act_tbl[index]._off, a2SAmaHeader.act_tbl[index]);
                }
                else
                    a2SAmaHeader.act_tbl[index] = AppMain.readAMAFile_actHash[key];
            }
            for (int index = 0; (long)index < (long)a2SAmaHeader.act_num; ++index)
            {
                br.BaseStream.Seek((long)a2SAmaHeader.act_tbl[index]._off, SeekOrigin.Begin);
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
                    if (!AppMain.readAMAFile_mtnHash.ContainsKey(a2SAmaHeader.act_tbl[index].mtn_offset))
                    {
                        a2SAmaHeader.act_tbl[index].mtn = new AppMain.A2S_AMA_MTN();
                        AppMain.readAMAFile_mtnHash.Add(a2SAmaHeader.act_tbl[index].mtn_offset, a2SAmaHeader.act_tbl[index].mtn);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].mtn = AppMain.readAMAFile_mtnHash[a2SAmaHeader.act_tbl[index].mtn_offset];
                }
                a2SAmaHeader.act_tbl[index].anm_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].anm_offset != 0)
                {
                    if (!AppMain.readAMAFile_anmHash.ContainsKey(a2SAmaHeader.act_tbl[index].anm_offset))
                    {
                        a2SAmaHeader.act_tbl[index].anm = new AppMain.A2S_AMA_ANM();
                        AppMain.readAMAFile_anmHash.Add(a2SAmaHeader.act_tbl[index].anm_offset, a2SAmaHeader.act_tbl[index].anm);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].anm = AppMain.readAMAFile_anmHash[a2SAmaHeader.act_tbl[index].anm_offset];
                }
                a2SAmaHeader.act_tbl[index].acm_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].acm_offset != 0)
                {
                    if (!AppMain.readAMAFile_acmHash.ContainsKey(a2SAmaHeader.act_tbl[index].acm_offset))
                    {
                        a2SAmaHeader.act_tbl[index].acm = new AppMain.A2S_AMA_ACM();
                        AppMain.readAMAFile_acmHash.Add(a2SAmaHeader.act_tbl[index].acm_offset, a2SAmaHeader.act_tbl[index].acm);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].acm = AppMain.readAMAFile_acmHash[a2SAmaHeader.act_tbl[index].acm_offset];
                }
                a2SAmaHeader.act_tbl[index].usr_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].usr_offset != 0)
                {
                    if (!AppMain.readAMAFile_usrHash.ContainsKey(a2SAmaHeader.act_tbl[index].usr_offset))
                    {
                        a2SAmaHeader.act_tbl[index].usr = new AppMain.A2S_AMA_USR();
                        AppMain.readAMAFile_usrHash.Add(a2SAmaHeader.act_tbl[index].usr_offset, a2SAmaHeader.act_tbl[index].usr);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].usr = AppMain.readAMAFile_usrHash[a2SAmaHeader.act_tbl[index].usr_offset];
                }
                a2SAmaHeader.act_tbl[index].hit_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].hit_offset != 0)
                {
                    if (!AppMain.readAMAFile_hitHash.ContainsKey(a2SAmaHeader.act_tbl[index].hit_offset))
                    {
                        a2SAmaHeader.act_tbl[index].hit = new AppMain.A2S_AMA_HIT();
                        AppMain.readAMAFile_hitHash.Add(a2SAmaHeader.act_tbl[index].hit_offset, a2SAmaHeader.act_tbl[index].hit);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].hit = AppMain.readAMAFile_hitHash[a2SAmaHeader.act_tbl[index].hit_offset];
                }
                a2SAmaHeader.act_tbl[index].next_offset = br.ReadInt32();
                if (a2SAmaHeader.act_tbl[index].next_offset != 0)
                {
                    if (!AppMain.readAMAFile_actHash.ContainsKey(a2SAmaHeader.act_tbl[index].next_offset))
                    {
                        a2SAmaHeader.act_tbl[index].next = new AppMain.A2S_AMA_ACT();
                        AppMain.readAMAFile_actHash.Add(a2SAmaHeader.act_tbl[index].next_offset, a2SAmaHeader.act_tbl[index].next);
                    }
                    else
                        a2SAmaHeader.act_tbl[index].next = AppMain.readAMAFile_actHash[a2SAmaHeader.act_tbl[index].next_offset];
                }
                br.BaseStream.Seek(8L, SeekOrigin.Current);
            }
            br.BaseStream.Seek((long)a2SAmaHeader.act_name_tbl_offset, SeekOrigin.Begin);
            int[] numArray = new int[(int)a2SAmaHeader.act_num];
            for (int index = 0; (long)index < (long)a2SAmaHeader.act_num; ++index)
                numArray[index] = br.ReadInt32();
            for (int index = 0; (long)index < (long)a2SAmaHeader.act_num; ++index)
            {
                br.BaseStream.Seek((long)numArray[index], SeekOrigin.Begin);
                AppMain.skipString(br);
            }
            foreach (KeyValuePair<int, AppMain.A2S_AMA_MTN> keyValuePair in AppMain.readAMAFile_mtnHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_AMA_MTN a2SAmaMtn = keyValuePair.Value;
                a2SAmaMtn.flag = br.ReadUInt32();
                a2SAmaMtn.mtn_key_num = br.ReadUInt32();
                a2SAmaMtn.mtn_frm_num = br.ReadUInt32();
                a2SAmaMtn.mtn_key_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.mtn_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaMtn.mtn_key_tbl_offset))
                    {
                        a2SAmaMtn.mtn_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaMtn.mtn_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaMtn.mtn_key_tbl_offset, a2SAmaMtn.mtn_key_tbl);
                    }
                    else
                        a2SAmaMtn.mtn_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaMtn.mtn_key_tbl_offset];
                }
                a2SAmaMtn.mtn_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.mtn_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_submtnHash.ContainsKey(a2SAmaMtn.mtn_tbl_offset))
                    {
                        a2SAmaMtn.mtn_tbl = new AppMain.A2S_SUB_MTN[(int)(a2SAmaMtn.mtn_key_num + 1U)];
                        AppMain.readAMAFile_submtnHash.Add(a2SAmaMtn.mtn_tbl_offset, a2SAmaMtn.mtn_tbl);
                    }
                    else
                        a2SAmaMtn.mtn_tbl = AppMain.readAMAFile_submtnHash[a2SAmaMtn.mtn_tbl_offset];
                }
                a2SAmaMtn.trs_key_num = br.ReadUInt32();
                a2SAmaMtn.trs_frm_num = br.ReadUInt32();
                a2SAmaMtn.trs_key_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.trs_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaMtn.trs_key_tbl_offset))
                    {
                        a2SAmaMtn.trs_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaMtn.trs_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaMtn.trs_key_tbl_offset, a2SAmaMtn.trs_key_tbl);
                    }
                    else
                        a2SAmaMtn.trs_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaMtn.trs_key_tbl_offset];
                }
                a2SAmaMtn.trs_tbl_offset = br.ReadInt32();
                if (a2SAmaMtn.trs_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subtrsHash.ContainsKey(a2SAmaMtn.trs_tbl_offset))
                    {
                        a2SAmaMtn.trs_tbl = new AppMain.A2S_SUB_TRS[(int)(a2SAmaMtn.trs_key_num + 1U)];
                        AppMain.readAMAFile_subtrsHash.Add(a2SAmaMtn.trs_tbl_offset, a2SAmaMtn.trs_tbl);
                    }
                    else
                        a2SAmaMtn.trs_tbl = AppMain.readAMAFile_subtrsHash[a2SAmaMtn.trs_tbl_offset];
                }
            }
            foreach (KeyValuePair<int, AppMain.A2S_AMA_ANM> keyValuePair in AppMain.readAMAFile_anmHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_AMA_ANM a2SAmaAnm = keyValuePair.Value;
                a2SAmaAnm.flag = br.ReadUInt32();
                a2SAmaAnm.anm_key_num = br.ReadUInt32();
                a2SAmaAnm.anm_frm_num = br.ReadUInt32();
                a2SAmaAnm.anm_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.anm_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaAnm.anm_key_tbl_offset))
                    {
                        a2SAmaAnm.anm_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaAnm.anm_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaAnm.anm_key_tbl_offset, a2SAmaAnm.anm_key_tbl);
                    }
                    else
                        a2SAmaAnm.anm_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaAnm.anm_key_tbl_offset];
                }
                a2SAmaAnm.anm_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.anm_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subanmHash.ContainsKey(a2SAmaAnm.anm_tbl_offset))
                    {
                        a2SAmaAnm.anm_tbl = new AppMain.A2S_SUB_ANM[(int)(a2SAmaAnm.anm_key_num + 1U)];
                        AppMain.readAMAFile_subanmHash.Add(a2SAmaAnm.anm_tbl_offset, a2SAmaAnm.anm_tbl);
                    }
                    else
                        a2SAmaAnm.anm_tbl = AppMain.readAMAFile_subanmHash[a2SAmaAnm.anm_tbl_offset];
                }
                a2SAmaAnm.mat_key_num = br.ReadUInt32();
                a2SAmaAnm.mat_frm_num = br.ReadUInt32();
                a2SAmaAnm.mat_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.mat_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaAnm.mat_key_tbl_offset))
                    {
                        a2SAmaAnm.mat_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaAnm.mat_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaAnm.mat_key_tbl_offset, a2SAmaAnm.mat_key_tbl);
                    }
                    else
                        a2SAmaAnm.mat_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaAnm.mat_key_tbl_offset];
                }
                a2SAmaAnm.mat_tbl_offset = br.ReadInt32();
                if (a2SAmaAnm.mat_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_submatHash.ContainsKey(a2SAmaAnm.mat_tbl_offset))
                    {
                        a2SAmaAnm.mat_tbl = new AppMain.A2S_SUB_MAT[(int)(a2SAmaAnm.mat_key_num + 1U)];
                        AppMain.readAMAFile_submatHash.Add(a2SAmaAnm.mat_tbl_offset, a2SAmaAnm.mat_tbl);
                    }
                    else
                        a2SAmaAnm.mat_tbl = AppMain.readAMAFile_submatHash[a2SAmaAnm.mat_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, AppMain.A2S_AMA_ACM> keyValuePair in AppMain.readAMAFile_acmHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_AMA_ACM a2SAmaAcm = keyValuePair.Value;
                a2SAmaAcm.flag = br.ReadUInt32();
                a2SAmaAcm.acm_key_num = br.ReadUInt32();
                a2SAmaAcm.acm_frm_num = br.ReadUInt32();
                a2SAmaAcm.acm_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.acm_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaAcm.acm_key_tbl_offset))
                    {
                        a2SAmaAcm.acm_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaAcm.acm_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaAcm.acm_key_tbl_offset, a2SAmaAcm.acm_key_tbl);
                    }
                    else
                        a2SAmaAcm.acm_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaAcm.acm_key_tbl_offset];
                }
                a2SAmaAcm.acm_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.acm_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subacmHash.ContainsKey(a2SAmaAcm.acm_tbl_offset))
                    {
                        a2SAmaAcm.acm_tbl = new AppMain.A2S_SUB_ACM[(int)(a2SAmaAcm.acm_key_num + 1U)];
                        AppMain.readAMAFile_subacmHash.Add(a2SAmaAcm.acm_tbl_offset, a2SAmaAcm.acm_tbl);
                    }
                    else
                        a2SAmaAcm.acm_tbl = AppMain.readAMAFile_subacmHash[a2SAmaAcm.acm_tbl_offset];
                }
                a2SAmaAcm.trs_key_num = br.ReadUInt32();
                a2SAmaAcm.trs_frm_num = br.ReadUInt32();
                a2SAmaAcm.trs_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.trs_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaAcm.trs_key_tbl_offset))
                    {
                        a2SAmaAcm.trs_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaAcm.trs_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaAcm.trs_key_tbl_offset, a2SAmaAcm.trs_key_tbl);
                    }
                    else
                        a2SAmaAcm.trs_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaAcm.trs_key_tbl_offset];
                }
                a2SAmaAcm.trs_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.trs_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subtrsHash.ContainsKey(a2SAmaAcm.trs_tbl_offset))
                    {
                        a2SAmaAcm.trs_tbl = new AppMain.A2S_SUB_TRS[(int)(a2SAmaAcm.trs_key_num + 1U)];
                        AppMain.readAMAFile_subtrsHash.Add(a2SAmaAcm.trs_tbl_offset, a2SAmaAcm.trs_tbl);
                    }
                    else
                        a2SAmaAcm.trs_tbl = AppMain.readAMAFile_subtrsHash[a2SAmaAcm.trs_tbl_offset];
                }
                a2SAmaAcm.mat_key_num = br.ReadUInt32();
                a2SAmaAcm.mat_frm_num = br.ReadUInt32();
                a2SAmaAcm.mat_key_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.mat_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaAcm.mat_key_tbl_offset))
                    {
                        a2SAmaAcm.mat_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaAcm.mat_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaAcm.mat_key_tbl_offset, a2SAmaAcm.mat_key_tbl);
                    }
                    else
                        a2SAmaAcm.mat_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaAcm.mat_key_tbl_offset];
                }
                a2SAmaAcm.mat_tbl_offset = br.ReadInt32();
                if (a2SAmaAcm.mat_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_submatHash.ContainsKey(a2SAmaAcm.mat_tbl_offset))
                    {
                        a2SAmaAcm.mat_tbl = new AppMain.A2S_SUB_MAT[(int)(a2SAmaAcm.mat_key_num + 1U)];
                        AppMain.readAMAFile_submatHash.Add(a2SAmaAcm.mat_tbl_offset, a2SAmaAcm.mat_tbl);
                    }
                    else
                        a2SAmaAcm.mat_tbl = AppMain.readAMAFile_submatHash[a2SAmaAcm.mat_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, AppMain.A2S_AMA_USR> keyValuePair in AppMain.readAMAFile_usrHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_AMA_USR a2SAmaUsr = keyValuePair.Value;
                a2SAmaUsr.flag = br.ReadUInt32();
                a2SAmaUsr.usr_key_num = br.ReadUInt32();
                a2SAmaUsr.usr_frm_num = br.ReadUInt32();
                a2SAmaUsr.usr_key_tbl_offset = br.ReadInt32();
                if (a2SAmaUsr.usr_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaUsr.usr_key_tbl_offset))
                    {
                        a2SAmaUsr.usr_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaUsr.usr_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaUsr.usr_key_tbl_offset, a2SAmaUsr.usr_key_tbl);
                    }
                    else
                        a2SAmaUsr.usr_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaUsr.usr_key_tbl_offset];
                }
                a2SAmaUsr.usr_tbl_offset = br.ReadInt32();
                if (a2SAmaUsr.usr_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subusrHash.ContainsKey(a2SAmaUsr.usr_tbl_offset))
                    {
                        a2SAmaUsr.usr_tbl = new AppMain.A2S_SUB_USR[(int)(a2SAmaUsr.usr_key_num + 1U)];
                        AppMain.readAMAFile_subusrHash.Add(a2SAmaUsr.usr_tbl_offset, a2SAmaUsr.usr_tbl);
                    }
                    else
                        a2SAmaUsr.usr_tbl = AppMain.readAMAFile_subusrHash[a2SAmaUsr.usr_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, AppMain.A2S_AMA_HIT> keyValuePair in AppMain.readAMAFile_hitHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_AMA_HIT a2SAmaHit = keyValuePair.Value;
                a2SAmaHit.flag = br.ReadUInt32();
                a2SAmaHit.hit_key_num = br.ReadUInt32();
                a2SAmaHit.hit_frm_num = br.ReadUInt32();
                a2SAmaHit.hit_key_tbl_offset = br.ReadInt32();
                if (a2SAmaHit.hit_key_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subkeyHash.ContainsKey(a2SAmaHit.hit_key_tbl_offset))
                    {
                        a2SAmaHit.hit_key_tbl = new AppMain.A2S_SUB_KEY[(int)(a2SAmaHit.hit_key_num + 1U)];
                        AppMain.readAMAFile_subkeyHash.Add(a2SAmaHit.hit_key_tbl_offset, a2SAmaHit.hit_key_tbl);
                    }
                    else
                        a2SAmaHit.hit_key_tbl = AppMain.readAMAFile_subkeyHash[a2SAmaHit.hit_key_tbl_offset];
                }
                a2SAmaHit.hit_tbl_offset = br.ReadInt32();
                if (a2SAmaHit.hit_tbl_offset != 0)
                {
                    if (!AppMain.readAMAFile_subhitHash.ContainsKey(a2SAmaHit.hit_tbl_offset))
                    {
                        a2SAmaHit.hit_tbl = new AppMain.A2S_SUB_HIT[(int)(a2SAmaHit.hit_key_num + 1U)];
                        AppMain.readAMAFile_subhitHash.Add(a2SAmaHit.hit_tbl_offset, a2SAmaHit.hit_tbl);
                    }
                    else
                        a2SAmaHit.hit_tbl = AppMain.readAMAFile_subhitHash[a2SAmaHit.hit_tbl_offset];
                }
                br.BaseStream.Seek(12L, SeekOrigin.Current);
            }
            foreach (KeyValuePair<int, AppMain.A2S_SUB_TRS[]> keyValuePair in AppMain.readAMAFile_subtrsHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_SUB_TRS[] a2SSubTrsArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubTrsArray[index] = new AppMain.A2S_SUB_TRS();
                    a2SSubTrsArray[index].trs_x = br.ReadSingle();
                    a2SSubTrsArray[index].trs_y = br.ReadSingle();
                    a2SSubTrsArray[index].trs_z = br.ReadSingle();
                    a2SSubTrsArray[index].trs_accele = br.ReadSingle();
                }
            }
            foreach (KeyValuePair<int, AppMain.A2S_SUB_MTN[]> keyValuePair in AppMain.readAMAFile_submtnHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_SUB_MTN[] a2SSubMtnArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubMtnArray[index] = new AppMain.A2S_SUB_MTN();
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
                foreach (KeyValuePair<int, AppMain.A2S_SUB_ANM[]> keyValuePair in AppMain.readAMAFile_subanmHash)
                {
                    br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                    AppMain.A2S_SUB_ANM[] a2SSubAnmArray = keyValuePair.Value;
                    int length = keyValuePair.Value.Length;
                    for (int index = 0; index < length; ++index)
                    {
                        a2SSubAnmArray[index] = new AppMain.A2S_SUB_ANM();
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
            foreach (KeyValuePair<int, AppMain.A2S_SUB_MAT[]> keyValuePair in AppMain.readAMAFile_submatHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_SUB_MAT[] a2SSubMatArray = keyValuePair.Value;
                int num = keyValuePair.Value.Length - 1;
                for (int index = 0; index < num; ++index)
                {
                    a2SSubMatArray[index] = new AppMain.A2S_SUB_MAT();
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
            foreach (KeyValuePair<int, AppMain.A2S_SUB_ACM[]> keyValuePair in AppMain.readAMAFile_subacmHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_SUB_ACM[] a2SSubAcmArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubAcmArray[index] = new AppMain.A2S_SUB_ACM();
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
            foreach (KeyValuePair<int, AppMain.A2S_SUB_USR[]> keyValuePair in AppMain.readAMAFile_subusrHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_SUB_USR[] a2SSubUsrArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubUsrArray[index].usr_id = br.ReadUInt32();
                    br.BaseStream.Seek(12L, SeekOrigin.Current);
                    a2SSubUsrArray[index].usr_accele = br.ReadSingle();
                    br.BaseStream.Seek(12L, SeekOrigin.Current);
                }
            }
            foreach (KeyValuePair<int, AppMain.A2S_SUB_HIT[]> keyValuePair in AppMain.readAMAFile_subhitHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_SUB_HIT[] a2SSubHitArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubHitArray[index] = new AppMain.A2S_SUB_HIT();
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
            foreach (KeyValuePair<int, AppMain.A2S_SUB_KEY[]> keyValuePair in AppMain.readAMAFile_subkeyHash)
            {
                br.BaseStream.Seek((long)keyValuePair.Key, SeekOrigin.Begin);
                AppMain.A2S_SUB_KEY[] a2SSubKeyArray = keyValuePair.Value;
                int length = keyValuePair.Value.Length;
                for (int index = 0; index < length; ++index)
                {
                    a2SSubKeyArray[index] = new AppMain.A2S_SUB_KEY();
                    a2SSubKeyArray[index].frm = br.ReadUInt32();
                    a2SSubKeyArray[index].interpol = br.ReadUInt32();
                }
            }
        }
        AppMain.readAMAFile_nodeHash.Clear();
        AppMain.readAMAFile_actHash.Clear();
        AppMain.readAMAFile_mtnHash.Clear();
        AppMain.readAMAFile_anmHash.Clear();
        AppMain.readAMAFile_acmHash.Clear();
        AppMain.readAMAFile_usrHash.Clear();
        AppMain.readAMAFile_hitHash.Clear();
        AppMain.readAMAFile_subtrsHash.Clear();
        AppMain.readAMAFile_submtnHash.Clear();
        AppMain.readAMAFile_subanmHash.Clear();
        AppMain.readAMAFile_submatHash.Clear();
        AppMain.readAMAFile_subacmHash.Clear();
        AppMain.readAMAFile_subusrHash.Clear();
        AppMain.readAMAFile_subhitHash.Clear();
        AppMain.readAMAFile_subkeyHash.Clear();
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
                AppMain.readChars_name[length] = ch;
                ++length;
            }
            else
                break;
        }
        return new string(AppMain.readChars_name, 0, length);
    }

}