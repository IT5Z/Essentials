using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Essentials.Model
{
    class ProtectInfo
    {
        public Vector3 position;
        public byte angle;
        public DateTime time;

        public ProtectInfo(Vector3 position, byte angle, DateTime time)
        {
            this.position = position;
            this.angle = angle;
            this.time = time;
        }
    }
}
