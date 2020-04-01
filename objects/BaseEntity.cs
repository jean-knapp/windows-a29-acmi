using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class BaseEntity
    {
        public BaseEntity parent = null;
        public List<BaseEntity> children = new List<BaseEntity>();

        public String hexId = "";
        protected string name = "";
        public virtual String Name {
            get { return name; }
            set { name = value; }
        }

        public double latitude = 0;
        public double longitude = 0;

        
        public BaseEntity()
        {
            
        }

        public string getImageSource()
        {
            return "viewer/objects/2d/default.png";
        }
    }
}
