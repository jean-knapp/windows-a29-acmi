using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using PMAFileAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace windows_a29_acmi
{
    public partial class Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private List<BaseEntity> entities = new List<BaseEntity>();
        private List<GroundThreat> threats = new List<GroundThreat>();

        String unitsElevation = "ft";

        public Main()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            updateEntitiesList();
        }

        private void addAircraft_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(selectAcmiFileDialog.ShowDialog() == DialogResult.OK)
            {
                Aircraft aircraft = A29FileReader.readMNGFile(selectAcmiFileDialog.FileName, this);
                if(aircraft != null)
                {
                    entities.Add(aircraft);
                    updateEntitiesList();
                }
            }
        }

        private void clearAircrafts_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach(Aircraft aircraft in getAircrafts())
            {
                entities.Remove(aircraft);
            }
        }

        private void entityList_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        { e.NodeImageIndex = e.Node.Id; }

        private List<Aircraft> getAircrafts()
        {
            List<Aircraft> result = new List<Aircraft>();
            foreach(BaseEntity entity in entities)
            {
                if(entity is Aircraft)
                {
                    result.Add((Aircraft)entity);
                }
            }
            return result;
        }

        private void importFromPMAButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                PMANode root = PMAFile.read(dialog.FileName);

                entities = importFromPMATraverse(root, null);

                updateEntitiesList();
            }
        }

        private List<BaseEntity> importFromPMATraverse(PMANode root, BaseEntity parent)
        {
            List<BaseEntity> entities = new List<BaseEntity>();
            foreach(PMANode node in root.items)
            {
                if(node.GetType() == typeof(PMAObject))
                {
                    PMAObject objectNode = (PMAObject)node;

                    //if (objectNode.getType() == "ObjetoCenario")
                    //{
                    switch(objectNode.getType())
                    {
                        case "Grupo de Objetos":
                        {
                            Folder entity = new Folder() { Name = node.name, parent = parent };
                            entity.children = importFromPMATraverse(node.getNodeByType("Filhos"), entity);
                            entities.Add(entity);
                        }
                            break;
                        case "ObjetoCenario":
                        {
                            BaseEntity entity = new BaseEntity()
                            {
                                Name = node.name,
                                parent = parent,
                                latitude = double.Parse(((PMAObject)node).getSpecific().properties["Posicao.Lat"]),
                                longitude = double.Parse(((PMAObject)node).getSpecific().properties["Posicao.Lon"])
                            };

                            entities.Add(entity);
                        }

                            break;
                        default:
                        {
                            BaseEntity entity = new BaseEntity() { Name = node.name, parent = parent };

                            entities.Add(entity);
                        }
                            break;
                    }

                    //}
                }
            }
            return entities;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            TreeListNode childNode = (TreeListNode)data.GetData(typeof(TreeListNode));
            if(childNode == null)
                return;
            TreeList list = (TreeList)sender;

            DXDragEventArgs args = list.GetDXDragEventArgs(e);
            DragInsertPosition position = args.DragInsertPosition;
            TreeListNode parentNode = args.TargetNode;
            int id;

            // Get drop position
            if(position == DragInsertPosition.Before || position == DragInsertPosition.After)
            {
                if(parentNode.ParentNode != null)
                    id = parentNode.ParentNode.Nodes.IndexOf(parentNode);
                else
                    id = list.Nodes.IndexOf(parentNode);

                parentNode = parentNode.ParentNode;
            } else
                id = parentNode.Nodes.Count;


            BaseEntity childEntity = (BaseEntity)childNode.Tag;
            BaseEntity parentEntity = (parentNode != null ? (BaseEntity)parentNode.Tag : null);

            // Remove node from old parent
            if(childEntity.parent != null)
                childEntity.parent.children.Remove(childEntity);
            else
                entities.Remove(childEntity);

            // Add node to new parent
            if(parentEntity != null)
                parentEntity.children.Insert(id, childEntity);
            else
                entities.Insert(id, childEntity);
            childEntity.parent = parentEntity;
        }

        private void ThreatsGroundImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(selectXMLFileDialog.ShowDialog() == DialogResult.OK)
            {
                threats = A29FileReader.readAVD_AREAFile(selectXMLFileDialog.FileName, this);
                MessageBox.Show(threats.Count + string.Empty);
            }
        }

        private void toTacview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(saveTacviewFileDialog.ShowDialog() == DialogResult.OK)
            {
                String directory = Path.GetDirectoryName(saveTacviewFileDialog.FileName);
                switch(new FileInfo(saveTacviewFileDialog.FileName).Extension)
                {
                    case ".acmi":
                        ACMIFileWriter.writeACMIFile(saveTacviewFileDialog.FileName, getAircrafts(), this);
                        break;
                }
            }
        }

        private void traverseEntitiesList(List<BaseEntity> entities, TreeListNode parentNode)
        {
            for(int i = 0; i < entities.Count; i++)
            {
                TreeListNode node = entityList.AppendNode(new object[] { entities[i].Name }, parentNode);
                node.Tag = entities[i];
                imageCollection1.AddImage(Image.FromFile("" +
                    entities[i].getImageSource()));

                if(entities[i] is Folder)
                {
                    traverseEntitiesList(((Folder)entities[i]).children, node);
                }
            }
        }

        private void updateEntitiesList()
        {
            imageCollection1.Clear();

            entityList.BeginUnboundLoad();
            entityList.Nodes.Clear();

            traverseEntitiesList(entities, null);


            entityList.EndUnboundLoad();
        }

        internal void setStatusCoordinates(double latitude, double longitude, double elevation)
        {
            statusCoordinateText.Caption = "Lat: " + latitude + ", Long: " + longitude;
            switch(unitsElevation)
            {
                case "ft":
                    statusElevationText.Caption = "Elev: " + (int)(elevation * 3.28084) + "ft";
                    break;
                case "m":
                default:
                    statusElevationText.Caption = "Elev: " + (int)elevation + "m";
                    break;
            }
        }

        internal List<GroundThreat> Threats { get => threats; set => threats = value; }
    }
}
