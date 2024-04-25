using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

public partial class AppMain
{
    public static AMS_AME_HEADER readAMEfile(string filename)
    {
        filename = string.Format("Content/{0}", filename);
        BinaryReader br = new BinaryReader(TitleContainer.OpenStream(filename));
        AMS_AME_HEADER amsAmeHeader = readAMEfile(br);
        br.Dispose();
        return amsAmeHeader;
    }

    public static AMS_AME_HEADER readAMEfile(AmbChunk data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data.array, data.offset, data.array.Length - data.offset))
        {
            using (BinaryReader br = new BinaryReader(memoryStream))
                return readAMEfile(br);
        }
    }

    private static AMS_AME_TEX_ANIM readTexAnim(BinaryReader br)
    {
        AMS_AME_TEX_ANIM amsAmeTexAnim = new AMS_AME_TEX_ANIM()
        {
            time = br.ReadSingle(),
            key_num = br.ReadInt32()
        };
        amsAmeTexAnim.key_buf = new AMS_AME_TEX_ANIM_KEY[amsAmeTexAnim.key_num];
        for (int index = 0; index < amsAmeTexAnim.key_num; ++index)
        {
            amsAmeTexAnim.key_buf[index].time = br.ReadSingle();
            amsAmeTexAnim.key_buf[index].l = br.ReadSingle();
            amsAmeTexAnim.key_buf[index].t = br.ReadSingle();
            amsAmeTexAnim.key_buf[index].r = br.ReadSingle();
            amsAmeTexAnim.key_buf[index].b = br.ReadSingle();
        }
        return amsAmeTexAnim;
    }

    private static void fillAMENodeBegin(
      AMS_AME_NODE node,
      short id,
      short type,
      uint flag,
      char[] name,
      int child_offset,
      int sibling_offset,
      int parent_offset)
    {
        node.id = id;
        node.type = type;
        node.flag = flag;
        Array.Copy(name, 0, node.name, 0, 12);
        node.child_offset = child_offset;
        node.sibling_offset = sibling_offset;
        node.parent_offset = parent_offset;
    }

    private static void readVector4(BinaryReader br, NNS_VECTOR4D v)
    {
        v.x = br.ReadSingle();
        v.y = br.ReadSingle();
        v.z = br.ReadSingle();
        v.w = br.ReadSingle();
    }

    private static void readQuaternion(BinaryReader br, ref NNS_QUATERNION q)
    {
        q.x = br.ReadSingle();
        q.y = br.ReadSingle();
        q.z = br.ReadSingle();
        q.w = br.ReadSingle();
    }

    public static AMS_AME_HEADER readAMEfile(BinaryReader br)
    {
        AMS_AME_HEADER amsAmeHeader = new AMS_AME_HEADER();
        amsAmeHeader.file_id = br.ReadBytes(4);
        amsAmeHeader.file_version = br.ReadInt32();
        amsAmeHeader.node_num = br.ReadInt32();
        amsAmeHeader.node_ofst = br.ReadUInt32();
        amsAmeHeader.bounding.center.x = br.ReadSingle();
        amsAmeHeader.bounding.center.y = br.ReadSingle();
        amsAmeHeader.bounding.center.z = br.ReadSingle();
        amsAmeHeader.bounding.center.w = br.ReadSingle();
        amsAmeHeader.bounding.radius = br.ReadSingle();
        amsAmeHeader.bounding.radius2 = br.ReadSingle();
        br.BaseStream.Seek(8L, SeekOrigin.Current);
        br.BaseStream.Seek(16L, SeekOrigin.Current);
        amsAmeHeader.node = new AMS_AME_NODE[amsAmeHeader.node_num];
        br.BaseStream.Seek(amsAmeHeader.node_ofst, SeekOrigin.Begin);
        Dictionary<uint, AMS_AME_NODE> dictionary = new Dictionary<uint, AMS_AME_NODE>(amsAmeHeader.node_num);
        for (int index1 = 0; index1 < amsAmeHeader.node_num; ++index1)
        {
            uint index2 = (uint)br.BaseStream.Position + 15U & 4294967280U;
            if (index2 < br.BaseStream.Length)
            {
                br.BaseStream.Seek(index2, SeekOrigin.Begin);
                short id = br.ReadInt16();
                AME_AME_NODE_TYPE ameAmeNodeType = (AME_AME_NODE_TYPE)br.ReadInt16();
                uint flag = br.ReadUInt32();
                char[] name = br.ReadChars(12);
                int child_offset = br.ReadInt32();
                int sibling_offset = br.ReadInt32();
                int parent_offset = br.ReadInt32();
                switch (ameAmeNodeType)
                {
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_OMNI:
                        AMS_AME_NODE_OMNI amsAmeNodeOmni = new AMS_AME_NODE_OMNI();
                        fillAMENodeBegin(amsAmeNodeOmni, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeOmni.translate);
                        readQuaternion(br, ref amsAmeNodeOmni.rotate);
                        amsAmeNodeOmni.inheritance_rate = br.ReadSingle();
                        amsAmeNodeOmni.life = br.ReadSingle();
                        amsAmeNodeOmni.start_time = br.ReadSingle();
                        amsAmeNodeOmni.offset = br.ReadSingle();
                        amsAmeNodeOmni.offset_chaos = br.ReadSingle();
                        amsAmeNodeOmni.speed = br.ReadSingle();
                        amsAmeNodeOmni.speed_chaos = br.ReadSingle();
                        amsAmeNodeOmni.max_count = br.ReadSingle();
                        amsAmeNodeOmni.frequency = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeOmni;
                        amsAmeHeader.node[index1] = amsAmeNodeOmni;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_DIRECTIONAL:
                        AMS_AME_NODE_DIRECTIONAL ameNodeDirectional = new AMS_AME_NODE_DIRECTIONAL();
                        fillAMENodeBegin(ameNodeDirectional, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, ameNodeDirectional.translate);
                        readQuaternion(br, ref ameNodeDirectional.rotate);
                        ameNodeDirectional.inheritance_rate = br.ReadSingle();
                        ameNodeDirectional.life = br.ReadSingle();
                        ameNodeDirectional.start_time = br.ReadSingle();
                        ameNodeDirectional.offset = br.ReadSingle();
                        ameNodeDirectional.offset_chaos = br.ReadSingle();
                        ameNodeDirectional.speed = br.ReadSingle();
                        ameNodeDirectional.speed_chaos = br.ReadSingle();
                        ameNodeDirectional.max_count = br.ReadSingle();
                        ameNodeDirectional.frequency = br.ReadSingle();
                        ameNodeDirectional.spread = br.ReadSingle();
                        ameNodeDirectional.spread_variation = br.ReadSingle();
                        dictionary[index2] = ameNodeDirectional;
                        amsAmeHeader.node[index1] = ameNodeDirectional;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_SURFACE:
                        AMS_AME_NODE_SURFACE amsAmeNodeSurface = new AMS_AME_NODE_SURFACE();
                        fillAMENodeBegin(amsAmeNodeSurface, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeSurface.translate);
                        readQuaternion(br, ref amsAmeNodeSurface.rotate);
                        amsAmeNodeSurface.inheritance_rate = br.ReadSingle();
                        amsAmeNodeSurface.life = br.ReadSingle();
                        amsAmeNodeSurface.start_time = br.ReadSingle();
                        amsAmeNodeSurface.offset = br.ReadSingle();
                        amsAmeNodeSurface.offset_chaos = br.ReadSingle();
                        amsAmeNodeSurface.speed = br.ReadSingle();
                        amsAmeNodeSurface.speed_chaos = br.ReadSingle();
                        amsAmeNodeSurface.max_count = br.ReadSingle();
                        amsAmeNodeSurface.frequency = br.ReadSingle();
                        amsAmeNodeSurface.width = br.ReadSingle();
                        amsAmeNodeSurface.width_variation = br.ReadSingle();
                        amsAmeNodeSurface.height = br.ReadSingle();
                        amsAmeNodeSurface.height_variation = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeSurface;
                        amsAmeHeader.node[index1] = amsAmeNodeSurface;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_CIRCLE:
                        AMS_AME_NODE_CIRCLE amsAmeNodeCircle = new AMS_AME_NODE_CIRCLE();
                        fillAMENodeBegin(amsAmeNodeCircle, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeCircle.translate);
                        readQuaternion(br, ref amsAmeNodeCircle.rotate);
                        amsAmeNodeCircle.inheritance_rate = br.ReadSingle();
                        amsAmeNodeCircle.life = br.ReadSingle();
                        amsAmeNodeCircle.start_time = br.ReadSingle();
                        amsAmeNodeCircle.offset = br.ReadSingle();
                        amsAmeNodeCircle.offset_chaos = br.ReadSingle();
                        amsAmeNodeCircle.speed = br.ReadSingle();
                        amsAmeNodeCircle.speed_chaos = br.ReadSingle();
                        amsAmeNodeCircle.max_count = br.ReadSingle();
                        amsAmeNodeCircle.frequency = br.ReadSingle();
                        amsAmeNodeCircle.spread = br.ReadSingle();
                        amsAmeNodeCircle.spread_variation = br.ReadSingle();
                        amsAmeNodeCircle.radius = br.ReadSingle();
                        amsAmeNodeCircle.radius_variation = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeCircle;
                        amsAmeHeader.node[index1] = amsAmeNodeCircle;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_SIMPLE_SPRITE:
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_SPRITE:
                        AMS_AME_NODE_SPRITE amsAmeNodeSprite = new AMS_AME_NODE_SPRITE();
                        fillAMENodeBegin(amsAmeNodeSprite, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeSprite.translate);
                        readQuaternion(br, ref amsAmeNodeSprite.rotate);
                        amsAmeNodeSprite.z_bias = br.ReadSingle();
                        amsAmeNodeSprite.inheritance_rate = br.ReadSingle();
                        amsAmeNodeSprite.life = br.ReadSingle();
                        amsAmeNodeSprite.start_time = br.ReadSingle();
                        amsAmeNodeSprite.size = br.ReadSingle();
                        amsAmeNodeSprite.size_chaos = br.ReadSingle();
                        amsAmeNodeSprite.scale_x_start = br.ReadSingle();
                        amsAmeNodeSprite.scale_x_end = br.ReadSingle();
                        amsAmeNodeSprite.scale_y_start = br.ReadSingle();
                        amsAmeNodeSprite.scale_y_end = br.ReadSingle();
                        amsAmeNodeSprite.twist_angle = br.ReadSingle();
                        amsAmeNodeSprite.twist_angle_chaos = br.ReadSingle();
                        amsAmeNodeSprite.twist_angle_speed = br.ReadSingle();
                        amsAmeNodeSprite.color_start.color = br.ReadUInt32();
                        amsAmeNodeSprite.color_end.color = br.ReadUInt32();
                        amsAmeNodeSprite.blend = br.ReadInt32();
                        amsAmeNodeSprite.texture_slot = br.ReadInt16();
                        amsAmeNodeSprite.texture_id = br.ReadInt16();
                        amsAmeNodeSprite.cropping_l = br.ReadSingle();
                        amsAmeNodeSprite.cropping_t = br.ReadSingle();
                        amsAmeNodeSprite.cropping_r = br.ReadSingle();
                        amsAmeNodeSprite.cropping_b = br.ReadSingle();
                        amsAmeNodeSprite.scroll_u = br.ReadSingle();
                        amsAmeNodeSprite.scroll_v = br.ReadSingle();
                        amsAmeNodeSprite.tex_anim = readTexAnim(br);
                        dictionary[index2] = amsAmeNodeSprite;
                        amsAmeHeader.node[index1] = amsAmeNodeSprite;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_LINE:
                        AMS_AME_NODE_LINE amsAmeNodeLine = new AMS_AME_NODE_LINE();
                        fillAMENodeBegin(amsAmeNodeLine, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeLine.translate);
                        readQuaternion(br, ref amsAmeNodeLine.rotate);
                        amsAmeNodeLine.z_bias = br.ReadSingle();
                        amsAmeNodeLine.inheritance_rate = br.ReadSingle();
                        amsAmeNodeLine.life = br.ReadSingle();
                        amsAmeNodeLine.start_time = br.ReadSingle();
                        amsAmeNodeLine.length_start = br.ReadSingle();
                        amsAmeNodeLine.length_end = br.ReadSingle();
                        amsAmeNodeLine.inside_width_start = br.ReadSingle();
                        amsAmeNodeLine.inside_width_end = br.ReadSingle();
                        amsAmeNodeLine.outside_width_start = br.ReadSingle();
                        amsAmeNodeLine.outside_width_end = br.ReadSingle();
                        amsAmeNodeLine.inside_color_start.color = br.ReadUInt32();
                        amsAmeNodeLine.inside_color_end.color = br.ReadUInt32();
                        amsAmeNodeLine.outside_color_start.color = br.ReadUInt32();
                        amsAmeNodeLine.outside_color_end.color = br.ReadUInt32();
                        amsAmeNodeLine.blend = br.ReadInt32();
                        amsAmeNodeLine.texture_slot = br.ReadInt16();
                        amsAmeNodeLine.texture_id = br.ReadInt16();
                        amsAmeNodeLine.cropping_l = br.ReadSingle();
                        amsAmeNodeLine.cropping_t = br.ReadSingle();
                        amsAmeNodeLine.cropping_r = br.ReadSingle();
                        amsAmeNodeLine.cropping_b = br.ReadSingle();
                        amsAmeNodeLine.scroll_u = br.ReadSingle();
                        amsAmeNodeLine.scroll_v = br.ReadSingle();
                        amsAmeNodeLine.tex_anim = readTexAnim(br);
                        dictionary[index2] = amsAmeNodeLine;
                        amsAmeHeader.node[index1] = amsAmeNodeLine;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_PLANE:
                        AMS_AME_NODE_PLANE amsAmeNodePlane = new AMS_AME_NODE_PLANE();
                        fillAMENodeBegin(amsAmeNodePlane, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodePlane.translate);
                        readQuaternion(br, ref amsAmeNodePlane.rotate);
                        readVector4(br, amsAmeNodePlane.rotate_axis);
                        amsAmeNodePlane.z_bias = br.ReadSingle();
                        amsAmeNodePlane.inheritance_rate = br.ReadSingle();
                        amsAmeNodePlane.life = br.ReadSingle();
                        amsAmeNodePlane.start_time = br.ReadSingle();
                        amsAmeNodePlane.size = br.ReadSingle();
                        amsAmeNodePlane.size_chaos = br.ReadSingle();
                        amsAmeNodePlane.scale_x_start = br.ReadSingle();
                        amsAmeNodePlane.scale_x_end = br.ReadSingle();
                        amsAmeNodePlane.scale_y_start = br.ReadSingle();
                        amsAmeNodePlane.scale_y_end = br.ReadSingle();
                        amsAmeNodePlane.color_start.color = br.ReadUInt32();
                        amsAmeNodePlane.color_end.color = br.ReadUInt32();
                        amsAmeNodePlane.blend = br.ReadInt32();
                        amsAmeNodePlane.texture_slot = br.ReadInt16();
                        amsAmeNodePlane.texture_id = br.ReadInt16();
                        amsAmeNodePlane.cropping_l = br.ReadSingle();
                        amsAmeNodePlane.cropping_t = br.ReadSingle();
                        amsAmeNodePlane.cropping_r = br.ReadSingle();
                        amsAmeNodePlane.cropping_b = br.ReadSingle();
                        amsAmeNodePlane.scroll_u = br.ReadSingle();
                        amsAmeNodePlane.scroll_v = br.ReadSingle();
                        amsAmeNodePlane.tex_anim = readTexAnim(br);
                        dictionary[index2] = amsAmeNodePlane;
                        amsAmeHeader.node[index1] = amsAmeNodePlane;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_MODEL:
                        AMS_AME_NODE_MODEL amsAmeNodeModel = new AMS_AME_NODE_MODEL();
                        fillAMENodeBegin(amsAmeNodeModel, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeModel.translate);
                        readQuaternion(br, ref amsAmeNodeModel.rotate);
                        readVector4(br, amsAmeNodeModel.rotate_axis);
                        readVector4(br, amsAmeNodeModel.scale_start);
                        readVector4(br, amsAmeNodeModel.scale_end);
                        amsAmeNodeModel.z_bias = br.ReadSingle();
                        amsAmeNodeModel.inheritance_rate = br.ReadSingle();
                        amsAmeNodeModel.life = br.ReadSingle();
                        amsAmeNodeModel.start_time = br.ReadSingle();
                        amsAmeNodeModel.model_name = br.ReadChars(8);
                        amsAmeNodeModel.lod = br.ReadInt32();
                        amsAmeNodeModel.color_start.color = br.ReadUInt32();
                        amsAmeNodeModel.color_end.color = br.ReadUInt32();
                        amsAmeNodeModel.blend = br.ReadInt32();
                        amsAmeNodeModel.scroll_u = br.ReadSingle();
                        amsAmeNodeModel.scroll_v = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeModel;
                        amsAmeHeader.node[index1] = amsAmeNodeModel;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_GRAVITY:
                        AMS_AME_NODE_GRAVITY amsAmeNodeGravity = new AMS_AME_NODE_GRAVITY();
                        fillAMENodeBegin(amsAmeNodeGravity, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeGravity.direction);
                        amsAmeNodeGravity.magnitude = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeGravity;
                        amsAmeHeader.node[index1] = amsAmeNodeGravity;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_UNIFORM:
                        AMS_AME_NODE_UNIFORM amsAmeNodeUniform = new AMS_AME_NODE_UNIFORM();
                        fillAMENodeBegin(amsAmeNodeUniform, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeUniform.direction);
                        amsAmeNodeUniform.magnitude = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeUniform;
                        amsAmeHeader.node[index1] = amsAmeNodeUniform;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_RADIAL:
                        AMS_AME_NODE_RADIAL amsAmeNodeRadial = new AMS_AME_NODE_RADIAL();
                        fillAMENodeBegin(amsAmeNodeRadial, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeRadial.position);
                        amsAmeNodeRadial.magnitude = br.ReadSingle();
                        amsAmeNodeRadial.attenuation = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeRadial;
                        amsAmeHeader.node[index1] = amsAmeNodeRadial;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_VORTEX:
                        AMS_AME_NODE_VORTEX amsAmeNodeVortex = new AMS_AME_NODE_VORTEX();
                        fillAMENodeBegin(amsAmeNodeVortex, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeVortex.position);
                        readVector4(br, amsAmeNodeVortex.axis);
                        dictionary[index2] = amsAmeNodeVortex;
                        amsAmeHeader.node[index1] = amsAmeNodeVortex;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_DRAG:
                        AMS_AME_NODE_DRAG amsAmeNodeDrag = new AMS_AME_NODE_DRAG();
                        fillAMENodeBegin(amsAmeNodeDrag, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeDrag.position);
                        amsAmeNodeDrag.magnitude = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeDrag;
                        amsAmeHeader.node[index1] = amsAmeNodeDrag;
                        break;
                    case AME_AME_NODE_TYPE.AME_AME_NODE_TYPE_NOISE:
                        AMS_AME_NODE_NOISE amsAmeNodeNoise = new AMS_AME_NODE_NOISE();
                        fillAMENodeBegin(amsAmeNodeNoise, id, (short)ameAmeNodeType, flag, name, child_offset, sibling_offset, parent_offset);
                        readVector4(br, amsAmeNodeNoise.axis);
                        amsAmeNodeNoise.magnitude = br.ReadSingle();
                        dictionary[index2] = amsAmeNodeNoise;
                        amsAmeHeader.node[index1] = amsAmeNodeNoise;
                        break;
                }
            }
            else
                break;
        }
        foreach (KeyValuePair<uint, AMS_AME_NODE> keyValuePair in dictionary)
        {
            AMS_AME_NODE amsAmeNode = keyValuePair.Value;
            if (amsAmeNode.child_offset != 0)
                amsAmeNode.child = dictionary[(uint)amsAmeNode.child_offset];
            if (amsAmeNode.parent_offset != 0)
                amsAmeNode.parent = dictionary[(uint)amsAmeNode.parent_offset];
            if (amsAmeNode.sibling_offset != 0)
                amsAmeNode.sibling = dictionary[(uint)amsAmeNode.sibling_offset];
        }
        return amsAmeHeader;
    }

    private static int amAMEConv(byte[] pFile)
    {
        mppAssertNotImpl();
        return 1;
    }


}