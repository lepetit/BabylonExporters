using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Max;
using ActionItem = Autodesk.Max.Plugins.ActionItem;

namespace Max2Babylon
{
    class BabylonSaveAnimations:ActionItem
    {

        public override bool ExecuteAction()
        {
            Tools.InitializeGuidNodesMap();
            var selectedContainers = Tools.GetContainerInSelection();

            if (selectedContainers.Count <= 0)
            {
                AnimationGroupList.SaveDataToAnimationHelper();
                return true;
            }

            foreach (IIContainerObject containerObject in selectedContainers)
            {
                AnimationGroupList.SaveDataToContainerHelper(containerObject);
            }

            return true;
        }

        public void Close()
        {
            return;
        }

        public override int Id_ => 4;

        public override string ButtonText
        {
            get { return "VrMur Store AnimationGroups..."; }
        }

        public override string MenuText
        {
            get
            {
                var selectedContainers = Tools.GetContainerInSelection();
                if (selectedContainers?.Count > 0)
                {
                    return "&VrMur Store AnimationGroups to selected containers...";
                }
                else
                {
                    return "&(Xref/Merge) VrMur Store AnimationGroups";
                }
            }
        }

        public override string DescriptionText
        {
            get { return "Babylon - Copy AnimationGroups into a BabylonAnimationHelper or a BabylonContainerHelper"; }
        }

        public override string CategoryText
        {
            get { return "VrMur"; }
        }

        public override bool IsChecked_
        {
            get { return false; }
        }

        public override bool IsItemVisible
        {
            get { return true; }
        }

        public override bool IsEnabled_
        {
            get { return true; }
        }
    }

}
