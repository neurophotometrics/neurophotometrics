using Bonsai;
using System;
using System.ComponentModel.Design;

namespace Neurophotometrics.Design
{
    class GroupRegionsEditor : CollectionEditor
    {
        public GroupRegionsEditor(Type type)
            : base(type)
        {
        }

        protected override string GetDisplayText(object value)
        {
            var group = (GroupedRegions)value;
            var name = string.IsNullOrEmpty(group.Name) ? "Group" : group.Name;
            var regions = group.Regions;
            if (regions == null || regions.Length == 0) return name;
            else return string.Format("{0} ({1})", name, ArrayConvert.ToString(group.Regions));
        }
    }
}
