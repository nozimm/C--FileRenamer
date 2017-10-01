using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication2 {
    public partial class Form1 : Form {

        public String path;
        public String fileName = "";
        public String extension = "";
        List<String> paths = new List<String>();
        List<String> newPaths = new List<String>();
        public String Number = "";
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        public Form1() {
            InitializeComponent();
            this.AllowDrop = true;

        }


        private void addData() {
            dataGrid.Rows.Add(1 + paths.Count - dataGrid.Rows.Count);

            for (int i = 0; i < newPaths.Count; i++) {
                path = newPaths[i];
                fileName = Path.GetFileNameWithoutExtension(path);
                extension = Path.GetExtension(path);

                //Calculate Nr
                String lengthPaths = "" + newPaths.Count;
                String length = "D" + lengthPaths.Length;
                String zerosNr = i.ToString(length);


                dataGrid.Rows[i].Cells[0].Value = "0" + zerosNr;
                dataGrid.Rows[i].Cells[1].Value = fileName;
                dataGrid.Rows[i].Cells[2].Value = extension;
                dataGrid.Rows[i].Cells[3].Value = path;
            }

        }





        private void btnRename_Click(object sender, EventArgs e) {
            if (cbNumber.Checked == true) {
                //Nummeriert
                for (int i = 0; i < paths.Count; i++) {
                    Number = Path.GetDirectoryName(newPaths[i])+ @"\" + dataGrid.Rows[i].Cells[0].FormattedValue.ToString() +"_"  
                        + Path.GetFileNameWithoutExtension(newPaths[i]) + Path.GetExtension(newPaths[i]);
                    try {
                        File.Move(paths[i], Number);
                    } catch {

                    }
                }

                } else {
                    //Not numbered
                for (int i = 0; i < paths.Count; i++) {


                    try {
                        File.Move(paths[i], newPaths[i]);
                    } catch {

                    }
                }
            }
        }


        private void btnAddFiles_Click(object sender, EventArgs e) {
            OpenFileDialog oFD1 = new OpenFileDialog();
            oFD1.Title = "Select a File";
            oFD1.InitialDirectory = @"Z:\files";
            oFD1.Multiselect = true;

            if (oFD1.ShowDialog() == DialogResult.OK) {
                for (int i = 0; i < oFD1.FileNames.Length; i++) {
                    paths.Add(oFD1.FileNames[i]);
                    newPaths.Add(oFD1.FileNames[1]);
                }
                addData();
            }

        }

 

        private void Form1_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.All;

        }

        private void Form1_DragDrop(object sender, DragEventArgs e) {
            try {
                String[] dragedFiles = e.Data.GetData(DataFormats.FileDrop) as string[];
                for (int i = 0; i < dragedFiles.Length; i++) {
                    paths.Add(dragedFiles[i]);
                    newPaths.Add(dragedFiles[i]);
                }
                addData();
            } catch (System.Exception) {

            }
        }


        private void dataGrid_MouseDown(object sender, MouseEventArgs e) {

            rowIndexFromMouseDown = dataGrid.HitTest(e.X, e.Y).RowIndex;

            if (rowIndexFromMouseDown != -1) {
          
                Size dragSize = SystemInformation.DragSize;


                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                                        dragSize);
            } else
                dragBoxFromMouseDown = Rectangle.Empty;

        }



        private void dataGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                  
                    DragDropEffects dropEffect = dataGrid.DoDragDrop(
                             dataGrid.Rows[rowIndexFromMouseDown],
                             DragDropEffects.Move);
                }
            }

        }

        private void dataGrid_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGrid_DragDrop(object sender, DragEventArgs e) {

            Point clientPoint = dataGrid.PointToClient(new Point(e.X, e.Y));

            rowIndexOfItemUnderMouseToDrop =
                dataGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Effect == DragDropEffects.Move) {
                DataGridViewRow rowToMove = e.Data.GetData(
                             typeof(DataGridViewRow)) as DataGridViewRow;
                try {
                    dataGrid.Rows.RemoveAt(rowIndexFromMouseDown);
                    dataGrid.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
                } catch (System.Exception) {
                    try {
                        dataGrid.Rows.Insert(rowIndexFromMouseDown, rowToMove);

                    } catch (System.Exception) {

                    }
                }

            }

        }

        private void btnFindReplace_Click(object sender, EventArgs e) {
            for (int i = 0; i < newPaths.Count; i++) {
                if (!txtFind.Text.Equals("") && !txtReplace.Equals("")) {
                    String t = newPaths[i];
                    String se = txtFind.Text;
                    String rw = txtReplace.Text;
                    t = Path.GetFileNameWithoutExtension(t);
                    t = t.Replace(se, rw);
                    newPaths[i] = Path.GetDirectoryName(newPaths[i]) + @"\" + t + Path.GetExtension(newPaths[i]);
                    dataGrid.Rows.Clear();
                    addData();
                } else {
                    if (txtFind.Text == null) {
                    }
                    if (txtFind == null) {
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            for (int i = 0; i < newPaths.Count; i++) {
                newPaths[i] = Path.GetDirectoryName(newPaths[i]) + @"\" + txtPrefix.Text + Path.GetFileNameWithoutExtension(newPaths[i]) + txtSuffix.Text + Path.GetExtension(newPaths[i]);
                dataGrid.Rows.Clear();
                addData();
            }
        }
    }
}
