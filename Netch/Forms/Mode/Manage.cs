using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Netch.Utils;

namespace Netch.Forms.Mode
{
    public partial class Manage : Form
    {
        private TreeNode defaultNode;
        private Process _modeForm;

        public Manage()
        {
            InitializeComponent();

            treeView1.Nodes.Clear();
            defaultNode = treeView1.Nodes.Add("Default", "Default");
            foreach (var mode in Global.Modes)
            {
                var split = mode.RelativePath.Split('\\');
                // split = split.Take(split.Length - 1).ToArray();

                var groupNode = split.Length > 1
                    ? AddDirectoryToTreeNode(treeView1.Nodes, split.Take(split.Length - 1).ToArray())
                    : defaultNode;

                groupNode.Nodes.Add(new TreeNode(mode.Remark) {Tag = mode});
            }
        }

        private TreeNode AddDirectoryToTreeNode(TreeNodeCollection rootNode, string[] folderNames)
        {
            TreeNode dirNode = null;

            while (folderNames.Length > 0)
            {
                dirNode = rootNode.Find(folderNames[0], false).FirstOrDefault()
                          ?? rootNode.Add(folderNames[0], folderNames[0]);

                rootNode = dirNode.Nodes;
                folderNames = folderNames.Skip(1).ToArray();
            }

            return dirNode;
        }


        private Process ModeForm
        {
            get => _modeForm;
            set
            {
                _modeForm = value;
                if (value == null) return;

                _modeForm.ModeForm_Load(null, null);
                _modeForm.FormClosed += (sender, args) =>
                {
                    _modeForm = null;
                    splitContainer1.Panel2.Controls.Clear();
                };
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!(e.Node.Tag is Models.Mode mode))
                return;

            if (mode.Type != 0)
                return;

            if (ModeForm != null)
            {
                if (ModeForm.IsEdited)
                {
                    switch (
                        MessageBox.Show(i18N.Translate("Mode have been edited,Saving?"), "", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.Yes:
                            ModeForm.ControlButton_Click(null, null);
                            if (ModeForm.IsDisposed == false)
                                return;
                            break;
                        case DialogResult.No:
                            ModeForm.Dispose();
                            break;
                    }
                }
                else
                {
                    ModeForm.Dispose();
                }
            }

            splitContainer1.Panel2.Controls.Clear();

            ModeForm = new Process(mode);
            splitContainer1.Panel2.Controls.Add(ModeForm.ConfigurationGroupBox);
            splitContainer1.Panel2.Controls.Add(ModeForm.ControlButton);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (ModeForm != null)
            {
                if (ModeForm.IsEdited)
                {
                    switch (
                        MessageBox.Show(i18N.Translate("Mode have been edited,Saving?"), "", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.Yes:
                            ModeForm.ControlButton_Click(null, null);
                            if (ModeForm.IsDisposed == false)
                                return;
                            break;
                        case DialogResult.No:
                            ModeForm.Dispose();
                            break;
                    }
                }
                else
                {
                    ModeForm.Dispose();
                }
            }

            splitContainer1.Panel2.Controls.Clear();

            ModeForm = new Process();
            splitContainer1.Panel2.Controls.Add(ModeForm.ConfigurationGroupBox);
            splitContainer1.Panel2.Controls.Add(ModeForm.ControlButton);
        }
    }
}