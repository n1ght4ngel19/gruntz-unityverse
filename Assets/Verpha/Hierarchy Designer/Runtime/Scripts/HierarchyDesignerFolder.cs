namespace Verpha.HierarchyDesigner
{
    using UnityEngine;

    public class HierarchyDesignerFolder : MonoBehaviour
    {
        [SerializeField] private bool flattenFolder = true;
        public bool ShouldFlatten => flattenFolder;

        private void Start()
        {
            FlattenFolderIfRequired();
        }

        private void FlattenFolderIfRequired()
        {
            if (flattenFolder)
            {
                RecursivelyFlatten(transform);
            }
        }

        private void RecursivelyFlatten(Transform folderTransform)
        {
            for (int i = folderTransform.childCount - 1; i >= 0; i--)
            {
                Transform childTransform = folderTransform.GetChild(i);
                HierarchyDesignerFolder childFolder = childTransform.GetComponent<HierarchyDesignerFolder>();

                if (childFolder != null)
                {
                    if (childFolder.ShouldFlatten)
                    {
                        childFolder.RecursivelyFlatten(childTransform);
                        Destroy(childTransform.gameObject);
                    }
                    else
                    {
                        childTransform.SetParent(null);
                    }
                }
                else
                {
                    childTransform.SetParent(null);
                }
            }

            Destroy(gameObject);
        }
    }
}