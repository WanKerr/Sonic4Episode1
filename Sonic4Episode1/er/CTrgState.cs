// Decompiled with JetBrains decompiler
// Type: er.CTrgState
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using accel;

namespace er
{
    public class CTrgState
    {
        private static int[] takeover = new int[4] { 14, 15, 16, 17 };
        private const int c_counter_none = -1;
        private const int c_wc_time_default = 6;
        private const int c_move_threshold_default = 2;
        private static readonly IntPair c_repeat_interval_default;
        private CCircularBuffer<CBitset> m_state;
        private int m_counter;
        private IntPair m_repeat_interval;
        private int m_wc_time;
        private int m_move_threshold;
        private IntPair m_move_accumulate;
        private IntPair m_move_report;

        public CTrgState()
        {
            this.m_state = new CCircularBuffer<CBitset>(2);
            this.m_counter = -1;
            this.m_repeat_interval = c_repeat_interval_default;
            this.m_wc_time = 6;
            this.m_move_threshold = 2;
            this.m_move_accumulate = new IntPair();
            this.m_move_report = new IntPair();
        }

        public virtual void Push(bool is_on, bool is_edge)
        {
            this.Push(is_on, is_edge, new IntPair());
        }

        public virtual void Push(bool is_on, bool is_edge, IntPair move)
        {
            this.updateTime();
            this.updateOnPrev(is_on);
            this.updateEdge(is_edge);
            this.updateRepeat();
            this.updateLock();
            this.updateMoveOver(move);
            this.updateClick();
        }

        public bool this[int kind]
        {
            get
            {
                return this.m_state[0][kind];
            }
        }

        public void AddLock()
        {
            this.m_state[0].set(14, true);
        }

        public void DelLock()
        {
            this.m_state[0].set(14, false);
        }

        public virtual void ResetState()
        {
            for (int index = 0; index < this.m_state.size(); ++index)
                this.m_state[index].reset();
        }

        public IntPair GetRepeatInterval()
        {
            return this.m_repeat_interval;
        }

        public int GetDoubleClickTime()
        {
            return this.m_wc_time;
        }

        public int GetMoveThreshold()
        {
            return this.m_move_threshold;
        }

        public IntPair GetMove()
        {
            return this.m_state[0][9] ? this.GetLastMove() : new IntPair();
        }

        public IntPair GetLastMove()
        {
            return this.m_move_report;
        }

        public IntPair GetOver()
        {
            return this.m_state[0][12] ? this.GetLastOver() : new IntPair();
        }

        public IntPair GetLastOver()
        {
            return this.m_move_report;
        }

        public void SetRepeatInterval(IntPair repeat_interval)
        {
            this.m_repeat_interval = repeat_interval;
        }

        public void SetRepeatInterval(int first, int second)
        {
            this.m_repeat_interval.first = first;
            this.m_repeat_interval.second = second;
        }

        public void SetRepeatInterval()
        {
            this.SetRepeatInterval(c_repeat_interval_default);
        }

        public void SetDoubleClickTime(int wc_time)
        {
            this.m_wc_time = wc_time;
        }

        public void SetDoubleClickTime()
        {
            this.SetDoubleClickTime(6);
        }

        public void SetMoveThreshold(int move_threshold)
        {
            this.m_move_threshold = move_threshold;
        }

        public void SetMoveThreshold()
        {
            this.SetMoveThreshold(2);
        }

        private void updateTime()
        {
            if (this.m_counter == 0)
            {
                this.m_counter = -1;
            }
            else
            {
                if (0 >= this.m_counter)
                    return;
                --this.m_counter;
            }
        }

        private void updateOnPrev(bool is_on)
        {
            if (this.m_state.size() == this.m_state.max_size())
            {
                CBitset back = this.m_state.back;
                back.reset();
                this.m_state.pop_back();
                this.m_state.push_front(back);
            }
            else
                this.m_state.push_front();
            CBitset at1 = this.m_state.getAt(0);
            CBitset at2 = this.m_state.getAt(1);
            if (is_on)
                at1.set(0, true);
            if (at2.test(0))
                at1.set(1, true);
            if (at2.test(10))
            {
                at1.set(16, at2.test(16));
            }
            else
            {
                for (int index = 0; index < takeover.Length; ++index)
                    at1.set(takeover[index], at2.test(takeover[index]));
            }
        }

        private void updateEdge(bool is_edge)
        {
            CBitset at = this.m_state.getAt(0);
            if (at.test(0))
            {
                if (this.m_state[1].test(0))
                    return;
                at.set(2, true);
                int pos = is_edge ? 8 : 11;
                at.set(pos, true);
            }
            else if (this.m_state[1].test(0))
            {
                at.set(3, true);
                int pos = is_edge ? 10 : 13;
                at.set(pos, true);
            }
            else
            {
                if (!at.test(14) || !is_edge)
                    return;
                at.set(10, true);
            }
        }

        private void updateRepeat()
        {
            CBitset at = this.m_state.getAt(0);
            if (at.test(2))
            {
                at.set(7, true);
                this.m_counter = this.m_repeat_interval.first;
            }
            else if (at.test(3))
            {
                this.m_counter = -1;
            }
            else
            {
                if (!at.test(0) || this.m_counter != 0)
                    return;
                at.set(7, true);
                this.m_counter = this.m_repeat_interval.second;
            }
        }

        private void updateLock()
        {
            CBitset at = this.m_state.getAt(0);
            if (at.test(8))
                at.set(14, true);
            if (!this.m_state[1].test(10))
                return;
            at.set(14, false);
        }

        private void updateMoveOver(IntPair move)
        {
            CBitset at = this.m_state.getAt(0);
            if (at.test(2))
            {
                at.set(12, true);
                if (at.test(14))
                    at.set(9, true);
                if (!this.m_state[1].test(14))
                    this.resetMove();
            }
            else if (at.test(0))
            {
                if (this.addMove(move))
                {
                    at.set(12, true);
                    if (at.test(14))
                    {
                        at.set(9, true);
                        at.set(15, true);
                    }
                }
            }
            else if (at.test(14) && this.addMove(move))
            {
                at.set(9, true);
                at.set(15, true);
            }
            if (!this.m_state[1].test(10))
                return;
            at.set(15, false);
        }

        private void updateClick()
        {
            CBitset at = this.m_state.getAt(0);
            if (at.test(8))
            {
                if (!at.test(16))
                    return;
                at.set(6, true);
                at.set(17, true);
                at.set(16, false);
                this.m_counter = -1;
            }
            else if (at.test(10))
            {
                if (!at.test(14) || !at.test(1) || at.test(17))
                    return;
                if (0 < this.m_wc_time)
                {
                    at.set(4, true);
                    at.set(16, true);
                    this.m_counter = this.m_wc_time;
                }
                else
                {
                    at.set(4, true);
                    at.set(5, true);
                }
            }
            else
            {
                if (at.test(0))
                    return;
                if (at.test(16) && this.m_counter == 0)
                {
                    at.set(5, true);
                    at.set(16, false);
                }
                if (!this.m_state[1].test(10))
                    return;
                at.set(17, false);
            }
        }

        private void resetMove()
        {
            this.m_move_accumulate = new IntPair();
            this.m_move_report = this.m_move_accumulate;
        }

        private bool addMove(IntPair move)
        {
            bool flag = false;
            this.m_move_accumulate.first += move.first;
            this.m_move_accumulate.second += move.second;
            if (this.m_move_threshold < Math.Abs(this.m_move_accumulate.first) || this.m_move_threshold < Math.Abs(this.m_move_accumulate.second))
                flag = true;
            if (flag)
            {
                this.m_move_report = this.m_move_accumulate;
                this.m_move_accumulate = new IntPair();
            }
            return flag;
        }

        static CTrgState()
        {
            c_repeat_interval_default.first = 15;
            c_repeat_interval_default.second = 6;
        }

        public class EState
        {
            public const int On = 0;
            public const int Prev = 1;
            public const int Stand = 2;
            public const int Release = 3;
            public const int Click = 4;
            public const int SingleClick = 5;
            public const int DoubleClick = 6;
            public const int Repeat = 7;
            public const int Down = 8;
            public const int Move = 9;
            public const int Up = 10;
            public const int In = 11;
            public const int Over = 12;
            public const int Out = 13;
            public const int Lock = 14;
            public const int DragAndDrop = 15;
            public const int DoubleClickWait = 16;
            public const int DoubleClickSecond = 17;
            public const int Max = 18;
            public const int None = 19;
        }

        public class BState : EState
        {
        }

        public enum ERepeatInterval
        {
            First,
            Second,
            Max,
            None,
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        private struct ETime
        {
            public const int Direct = 0;
            public const int Prev = 1;
            public const int Max = 2;
            public const int None = 3;
        }

        public class CBitset
        {
            private BitArray imp = new BitArray(18);

            public int Count => this.imp.Length;

            public bool this[int index]
            {
                get => this.imp[index];
                set => this.imp[index] = value;
            }

            public CBitset set(int pos)
            {
                this.imp.Set(pos, true);
                return this;
            }

            public CBitset set(int pos, bool value)
            {
                this.imp.Set(pos, value);
                return this;
            }

            public bool test(int pos)
            {
                return this.imp.Get(pos);
            }

            public void reset()
            {
                this.imp.SetAll(false);
            }
        }
    }
}
