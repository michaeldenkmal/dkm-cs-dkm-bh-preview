using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkm_cs_dkm_bh_preview
{
    public class ArrToCursor<T>
    {
        private int curIdx = 0;

        private T[] arr;

        public bool empty()
        {
            return arr.Length == 0;
        }

        public T Elem { get
            {
                return this.arr[curIdx];
            } 
        }

        public ArrToCursor(T[] arr) {
            this.arr = arr;
        }

        public void first()
        {
            curIdx =0;
        }

        public void last()
        {
            curIdx = this.arr.Length - 1;
        }

        public void next()
        {
            if (curIdx < this.arr.Length-1)
            {
                curIdx += 1;
            }
        }

        public void prev()
        {
            if (curIdx > 0)
            {
                curIdx -= 1;
            }
        }
        public bool bof()
        {
            return curIdx == 0;
        }

        public bool eof()
        {
            return curIdx == this.arr.Length-1;
        }
    }
}
