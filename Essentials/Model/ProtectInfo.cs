using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Essentials.Model
{
    struct ProtectInfo
    {
        public Vector3 position;
        public float rotation;
        public DateTime time;

        public ProtectInfo(Vector3 position, float angle, DateTime time)
        {
            this.position = position;
            this.rotation = angle;
            this.time = time;
        }
    }
}
