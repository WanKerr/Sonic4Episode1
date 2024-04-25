using accel;

public partial class AppMain
{
    public class CActionDraw
    {
        public CCircularBuffer<AOS_ACTION> m_action_array = new CCircularBuffer<AOS_ACTION>(100);

        public void Entry(A2S_AMA_HEADER ama, uint id, float frame, float x, float y)
        {
            if (!_am_sample_draw_enable)
                return;
            AOS_ACTION act = AoActCreate(ama, id, frame);
            AoActAcmPush();
            AoActAcmInit();
            AoActAcmApplyTrans(x, y, 0.0f);
            AoActUpdate(act, 0.0f);
            AoActSortRegAction(act);
            AoActAcmPop();
            this.m_action_array.push_back(act);
        }

        public void Entry(
          A2S_AMA_HEADER ama,
          uint id,
          float frame,
          float x,
          float y,
          float scalex,
          float scaley)
        {
            if (!_am_sample_draw_enable)
                return;
            AOS_ACTION act = AoActCreate(ama, id, frame);
            AoActAcmPush();
            AoActAcmInit();
            AoActAcmApplyTrans(x, y, 0.0f);
            AoActAcmApplyScale(scalex, scaley);
            AoActUpdate(act, 0.0f);
            AoActSortRegAction(act);
            AoActAcmPop();
            this.m_action_array.push_back(act);
        }

        public void Clear()
        {
            for (int index = 0; index < this.m_action_array.size(); ++index)
                AoActDelete(this.m_action_array[index]);
            this.m_action_array.clear();
        }

        public void Draw()
        {
            if (_am_sample_draw_enable)
            {
                AoActSortExecute();
                AoActSortDraw();
            }
            AoActSortUnregAll();
        }

        public void CActionDraw_destructor()
        {
            this.Clear();
        }
    }
}
