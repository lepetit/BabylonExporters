namespace Max2Babylon
{
    partial class ExporterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.butExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtModelPath = new System.Windows.Forms.RichTextBox();
            this.butModelBrowse = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.logTreeView = new System.Windows.Forms.TreeView();
            this.butCancel = new System.Windows.Forms.Button();
            this.chkManifest = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkWriteTextures = new System.Windows.Forms.CheckBox();
            this.chkExportAnimationsOnly = new System.Windows.Forms.CheckBox();
            this.chkExportTextures = new System.Windows.Forms.CheckBox();
            this.chkExportAnimations = new System.Windows.Forms.CheckBox();
            this.lblBakeAnimation = new System.Windows.Forms.Label();
            this.cmbBakeAnimationOptions = new System.Windows.Forms.ComboBox();
            this.chkApplyPreprocessToScene = new System.Windows.Forms.CheckBox();
            this.chkMrgContainersAndXref = new System.Windows.Forms.CheckBox();
            this.chkUsePreExportProces = new System.Windows.Forms.CheckBox();
            this.chkFlatten = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEnvironmentName = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnEnvBrowse = new System.Windows.Forms.Button();
            this.chkNoAutoLight = new System.Windows.Forms.CheckBox();
            this.textureLabel = new System.Windows.Forms.Label();
            this.txtTexturesPath = new System.Windows.Forms.RichTextBox();
            this.btnTxtBrowse = new System.Windows.Forms.Button();
            this.chkExportMaterials = new System.Windows.Forms.CheckBox();
            this.chkKHRMaterialsUnlit = new System.Windows.Forms.CheckBox();
            this.chkKHRTextureTransform = new System.Windows.Forms.CheckBox();
            this.chkKHRLightsPunctual = new System.Windows.Forms.CheckBox();
            this.chkOverwriteTextures = new System.Windows.Forms.CheckBox();
            this.chkDoNotOptimizeAnimations = new System.Windows.Forms.CheckBox();
            this.chkAnimgroupExportNonAnimated = new System.Windows.Forms.CheckBox();
            this.chkMergeAO = new System.Windows.Forms.CheckBox();
            this.txtQuality = new System.Windows.Forms.TextBox();
            this.labelQuality = new System.Windows.Forms.Label();
            this.chkExportMorphNormals = new System.Windows.Forms.CheckBox();
            this.chkExportMorphTangents = new System.Windows.Forms.CheckBox();
            this.chkExportTangents = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtScaleFactor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboOutputFormat = new System.Windows.Forms.ComboBox();
            this.chkOnlySelected = new System.Windows.Forms.CheckBox();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.chkHidden = new System.Windows.Forms.CheckBox();
            this.butClose = new System.Windows.Forms.Button();
            this.saveOptionBtn = new System.Windows.Forms.Button();
            this.envFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.butCopyToClipboard = new System.Windows.Forms.Button();
            this.exporterTabControl = new System.Windows.Forms.TabControl();
            this.exportOptionsTabPage = new System.Windows.Forms.TabPage();
            this.exportOptionsScrollPanel = new System.Windows.Forms.Panel();
            this.chkUseClone = new System.Windows.Forms.CheckBox();
            this.chkTryReuseTexture = new System.Windows.Forms.CheckBox();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.exporterTabControl.SuspendLayout();
            this.exportOptionsTabPage.SuspendLayout();
            this.exportOptionsScrollPanel.SuspendLayout();
            this.logTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // butExport
            // 
            this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butExport.Enabled = false;
            this.butExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butExport.Location = new System.Drawing.Point(345, 723);
            this.butExport.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.butExport.Name = "butExport";
            this.butExport.Size = new System.Drawing.Size(107, 28);
            this.butExport.TabIndex = 100;
            this.butExport.Text = "Export";
            this.butExport.UseVisualStyleBackColor = true;
            this.butExport.Click += new System.EventHandler(this.butExport_Click);
            this.butExport.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Model path:";
            // 
            // txtModelPath
            // 
            this.txtModelPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModelPath.Location = new System.Drawing.Point(129, 41);
            this.txtModelPath.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txtModelPath.Multiline = false;
            this.txtModelPath.Name = "txtModelPath";
            this.txtModelPath.Size = new System.Drawing.Size(789, 25);
            this.txtModelPath.TabIndex = 2;
            this.txtModelPath.Text = "";
            this.txtModelPath.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
            this.txtModelPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // butModelBrowse
            // 
            this.butModelBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butModelBrowse.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butModelBrowse.Location = new System.Drawing.Point(936, 41);
            this.butModelBrowse.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.butModelBrowse.Name = "butModelBrowse";
            this.butModelBrowse.Size = new System.Drawing.Size(27, 27);
            this.butModelBrowse.TabIndex = 3;
            this.butModelBrowse.Text = "бн";
            this.butModelBrowse.UseVisualStyleBackColor = true;
            this.butModelBrowse.Click += new System.EventHandler(this.butModelBrowse_Click);
            this.butModelBrowse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "gltf";
            this.saveFileDialog.Filter = "Gltf files|*.gltf";
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(8, 723);
            this.progressBar.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(327, 28);
            this.progressBar.TabIndex = 104;
            // 
            // logTreeView
            // 
            this.logTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTreeView.Location = new System.Drawing.Point(5, 7);
            this.logTreeView.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.logTreeView.Name = "logTreeView";
            this.logTreeView.Size = new System.Drawing.Size(1215, 563);
            this.logTreeView.TabIndex = 103;
            this.logTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Enabled = false;
            this.butCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butCancel.Location = new System.Drawing.Point(1000, 723);
            this.butCancel.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(107, 28);
            this.butCancel.TabIndex = 105;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            this.butCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkManifest
            // 
            this.chkManifest.AutoSize = true;
            this.chkManifest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkManifest.Location = new System.Drawing.Point(421, 193);
            this.chkManifest.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkManifest.Name = "chkManifest";
            this.chkManifest.Size = new System.Drawing.Size(137, 20);
            this.chkManifest.TabIndex = 14;
            this.chkManifest.Text = "Generate .manifest";
            this.chkManifest.UseVisualStyleBackColor = true;
            this.chkManifest.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 113);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Options:";
            // 
            // chkWriteTextures
            // 
            this.chkWriteTextures.AutoSize = true;
            this.chkWriteTextures.Checked = true;
            this.chkWriteTextures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteTextures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWriteTextures.Location = new System.Drawing.Point(19, 221);
            this.chkWriteTextures.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkWriteTextures.Name = "chkWriteTextures";
            this.chkWriteTextures.Size = new System.Drawing.Size(111, 20);
            this.chkWriteTextures.TabIndex = 11;
            this.chkWriteTextures.Text = "Write Textures";
            this.chkWriteTextures.UseVisualStyleBackColor = true;
            this.chkWriteTextures.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkExportAnimationsOnly
            // 
            this.chkExportAnimationsOnly.AutoSize = true;
            this.chkExportAnimationsOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExportAnimationsOnly.Location = new System.Drawing.Point(421, 251);
            this.chkExportAnimationsOnly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkExportAnimationsOnly.Name = "chkExportAnimationsOnly";
            this.chkExportAnimationsOnly.Size = new System.Drawing.Size(162, 20);
            this.chkExportAnimationsOnly.TabIndex = 43;
            this.chkExportAnimationsOnly.Text = "Export Animations Only";
            this.chkExportAnimationsOnly.UseVisualStyleBackColor = true;
            // 
            // chkExportTextures
            // 
            this.chkExportTextures.AutoSize = true;
            this.chkExportTextures.Checked = true;
            this.chkExportTextures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportTextures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExportTextures.Location = new System.Drawing.Point(216, 139);
            this.chkExportTextures.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkExportTextures.Name = "chkExportTextures";
            this.chkExportTextures.Size = new System.Drawing.Size(118, 20);
            this.chkExportTextures.TabIndex = 42;
            this.chkExportTextures.Text = "Export Textures";
            this.chkExportTextures.UseVisualStyleBackColor = true;
            // 
            // chkExportAnimations
            // 
            this.chkExportAnimations.AutoSize = true;
            this.chkExportAnimations.Checked = true;
            this.chkExportAnimations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportAnimations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExportAnimations.Location = new System.Drawing.Point(421, 139);
            this.chkExportAnimations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkExportAnimations.Name = "chkExportAnimations";
            this.chkExportAnimations.Size = new System.Drawing.Size(132, 20);
            this.chkExportAnimations.TabIndex = 42;
            this.chkExportAnimations.Text = "Export Animations";
            this.chkExportAnimations.UseVisualStyleBackColor = true;
            // 
            // lblBakeAnimation
            // 
            this.lblBakeAnimation.AutoSize = true;
            this.lblBakeAnimation.Enabled = false;
            this.lblBakeAnimation.Location = new System.Drawing.Point(212, 308);
            this.lblBakeAnimation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblBakeAnimation.Name = "lblBakeAnimation";
            this.lblBakeAnimation.Size = new System.Drawing.Size(157, 16);
            this.lblBakeAnimation.TabIndex = 40;
            this.lblBakeAnimation.Text = "Bake animations options:";
            // 
            // cmbBakeAnimationOptions
            // 
            this.cmbBakeAnimationOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBakeAnimationOptions.Enabled = false;
            this.cmbBakeAnimationOptions.Items.AddRange(new object[] {
            "Do not bake animations",
            "Bake all animations",
            "Selective bake"});
            this.cmbBakeAnimationOptions.Location = new System.Drawing.Point(413, 305);
            this.cmbBakeAnimationOptions.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.cmbBakeAnimationOptions.Name = "cmbBakeAnimationOptions";
            this.cmbBakeAnimationOptions.Size = new System.Drawing.Size(236, 24);
            this.cmbBakeAnimationOptions.TabIndex = 41;
            // 
            // chkApplyPreprocessToScene
            // 
            this.chkApplyPreprocessToScene.AutoSize = true;
            this.chkApplyPreprocessToScene.Enabled = false;
            this.chkApplyPreprocessToScene.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkApplyPreprocessToScene.Location = new System.Drawing.Point(19, 355);
            this.chkApplyPreprocessToScene.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkApplyPreprocessToScene.Name = "chkApplyPreprocessToScene";
            this.chkApplyPreprocessToScene.Size = new System.Drawing.Size(195, 20);
            this.chkApplyPreprocessToScene.TabIndex = 39;
            this.chkApplyPreprocessToScene.Text = "Apply Preprocess To Scene";
            this.chkApplyPreprocessToScene.UseVisualStyleBackColor = true;
            // 
            // chkMrgContainersAndXref
            // 
            this.chkMrgContainersAndXref.AutoSize = true;
            this.chkMrgContainersAndXref.Enabled = false;
            this.chkMrgContainersAndXref.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMrgContainersAndXref.Location = new System.Drawing.Point(19, 329);
            this.chkMrgContainersAndXref.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkMrgContainersAndXref.Name = "chkMrgContainersAndXref";
            this.chkMrgContainersAndXref.Size = new System.Drawing.Size(190, 20);
            this.chkMrgContainersAndXref.TabIndex = 37;
            this.chkMrgContainersAndXref.Text = "Merge Containers And XRef";
            this.chkMrgContainersAndXref.UseVisualStyleBackColor = true;
            // 
            // chkUsePreExportProces
            // 
            this.chkUsePreExportProces.AutoSize = true;
            this.chkUsePreExportProces.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkUsePreExportProces.Location = new System.Drawing.Point(19, 308);
            this.chkUsePreExportProces.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkUsePreExportProces.Name = "chkUsePreExportProces";
            this.chkUsePreExportProces.Size = new System.Drawing.Size(168, 20);
            this.chkUsePreExportProces.TabIndex = 36;
            this.chkUsePreExportProces.Text = "Use PreExport Process:";
            this.chkUsePreExportProces.UseVisualStyleBackColor = true;
            this.chkUsePreExportProces.CheckedChanged += new System.EventHandler(this.chkUsePreExportProces_CheckedChanged);
            // 
            // chkFlatten
            // 
            this.chkFlatten.AutoSize = true;
            this.chkFlatten.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFlatten.Location = new System.Drawing.Point(19, 279);
            this.chkFlatten.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkFlatten.Name = "chkFlatten";
            this.chkFlatten.Size = new System.Drawing.Size(137, 20);
            this.chkFlatten.TabIndex = 35;
            this.chkFlatten.Text = "Flatten Hierarchies";
            this.chkFlatten.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 493);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 16);
            this.label5.TabIndex = 29;
            this.label5.Text = "Environment";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 395);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 16);
            this.label8.TabIndex = 33;
            this.label8.Text = "Morph Target Options:";
            // 
            // txtEnvironmentName
            // 
            this.txtEnvironmentName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEnvironmentName.Location = new System.Drawing.Point(120, 491);
            this.txtEnvironmentName.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txtEnvironmentName.Multiline = false;
            this.txtEnvironmentName.Name = "txtEnvironmentName";
            this.txtEnvironmentName.Size = new System.Drawing.Size(777, 24);
            this.txtEnvironmentName.TabIndex = 30;
            this.txtEnvironmentName.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 516);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "GLTF Options:";
            // 
            // btnEnvBrowse
            // 
            this.btnEnvBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnvBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnvBrowse.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEnvBrowse.Location = new System.Drawing.Point(902, 489);
            this.btnEnvBrowse.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.btnEnvBrowse.Name = "btnEnvBrowse";
            this.btnEnvBrowse.Size = new System.Drawing.Size(27, 27);
            this.btnEnvBrowse.TabIndex = 31;
            this.btnEnvBrowse.Text = "бн";
            this.btnEnvBrowse.UseVisualStyleBackColor = true;
            this.btnEnvBrowse.Click += new System.EventHandler(this.btnEnvBrowse_Click);
            // 
            // chkNoAutoLight
            // 
            this.chkNoAutoLight.AutoSize = true;
            this.chkNoAutoLight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNoAutoLight.Location = new System.Drawing.Point(609, 137);
            this.chkNoAutoLight.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkNoAutoLight.Name = "chkNoAutoLight";
            this.chkNoAutoLight.Size = new System.Drawing.Size(136, 20);
            this.chkNoAutoLight.TabIndex = 27;
            this.chkNoAutoLight.Text = "No Automatic Light";
            this.chkNoAutoLight.UseVisualStyleBackColor = true;
            // 
            // textureLabel
            // 
            this.textureLabel.AutoSize = true;
            this.textureLabel.Location = new System.Drawing.Point(9, 85);
            this.textureLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.textureLabel.Name = "textureLabel";
            this.textureLabel.Size = new System.Drawing.Size(92, 16);
            this.textureLabel.TabIndex = 24;
            this.textureLabel.Text = "Textures Path:";
            // 
            // txtTexturesPath
            // 
            this.txtTexturesPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTexturesPath.Location = new System.Drawing.Point(129, 81);
            this.txtTexturesPath.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txtTexturesPath.Multiline = false;
            this.txtTexturesPath.Name = "txtTexturesPath";
            this.txtTexturesPath.Size = new System.Drawing.Size(789, 25);
            this.txtTexturesPath.TabIndex = 25;
            this.txtTexturesPath.Text = "";
            // 
            // btnTxtBrowse
            // 
            this.btnTxtBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTxtBrowse.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTxtBrowse.Location = new System.Drawing.Point(936, 81);
            this.btnTxtBrowse.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.btnTxtBrowse.Name = "btnTxtBrowse";
            this.btnTxtBrowse.Size = new System.Drawing.Size(27, 27);
            this.btnTxtBrowse.TabIndex = 26;
            this.btnTxtBrowse.Text = "бн";
            this.btnTxtBrowse.UseVisualStyleBackColor = true;
            this.btnTxtBrowse.Click += new System.EventHandler(this.btnTextureBrowse_Click);
            // 
            // chkExportMaterials
            // 
            this.chkExportMaterials.AutoSize = true;
            this.chkExportMaterials.Checked = true;
            this.chkExportMaterials.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportMaterials.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExportMaterials.Location = new System.Drawing.Point(19, 139);
            this.chkExportMaterials.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkExportMaterials.Name = "chkExportMaterials";
            this.chkExportMaterials.Size = new System.Drawing.Size(121, 20);
            this.chkExportMaterials.TabIndex = 23;
            this.chkExportMaterials.Text = "Export Materials";
            this.chkExportMaterials.UseVisualStyleBackColor = true;
            // 
            // chkKHRMaterialsUnlit
            // 
            this.chkKHRMaterialsUnlit.AutoSize = true;
            this.chkKHRMaterialsUnlit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkKHRMaterialsUnlit.Location = new System.Drawing.Point(421, 536);
            this.chkKHRMaterialsUnlit.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkKHRMaterialsUnlit.Name = "chkKHRMaterialsUnlit";
            this.chkKHRMaterialsUnlit.Size = new System.Drawing.Size(145, 20);
            this.chkKHRMaterialsUnlit.TabIndex = 22;
            this.chkKHRMaterialsUnlit.Text = "KHR_materials_unlit";
            this.chkKHRMaterialsUnlit.UseVisualStyleBackColor = true;
            // 
            // chkKHRTextureTransform
            // 
            this.chkKHRTextureTransform.AutoSize = true;
            this.chkKHRTextureTransform.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkKHRTextureTransform.Location = new System.Drawing.Point(216, 536);
            this.chkKHRTextureTransform.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkKHRTextureTransform.Name = "chkKHRTextureTransform";
            this.chkKHRTextureTransform.Size = new System.Drawing.Size(161, 20);
            this.chkKHRTextureTransform.TabIndex = 21;
            this.chkKHRTextureTransform.Text = "KHR_texture_transform";
            this.chkKHRTextureTransform.UseVisualStyleBackColor = true;
            // 
            // chkKHRLightsPunctual
            // 
            this.chkKHRLightsPunctual.AutoSize = true;
            this.chkKHRLightsPunctual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkKHRLightsPunctual.Location = new System.Drawing.Point(19, 536);
            this.chkKHRLightsPunctual.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkKHRLightsPunctual.Name = "chkKHRLightsPunctual";
            this.chkKHRLightsPunctual.Size = new System.Drawing.Size(148, 20);
            this.chkKHRLightsPunctual.TabIndex = 20;
            this.chkKHRLightsPunctual.Text = "KHR_lights_punctual";
            this.chkKHRLightsPunctual.UseVisualStyleBackColor = true;
            // 
            // chkOverwriteTextures
            // 
            this.chkOverwriteTextures.AutoSize = true;
            this.chkOverwriteTextures.Checked = true;
            this.chkOverwriteTextures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverwriteTextures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkOverwriteTextures.Location = new System.Drawing.Point(216, 221);
            this.chkOverwriteTextures.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkOverwriteTextures.Name = "chkOverwriteTextures";
            this.chkOverwriteTextures.Size = new System.Drawing.Size(136, 20);
            this.chkOverwriteTextures.TabIndex = 19;
            this.chkOverwriteTextures.Text = "Overwrite Textures";
            this.chkOverwriteTextures.UseVisualStyleBackColor = true;
            // 
            // chkDoNotOptimizeAnimations
            // 
            this.chkDoNotOptimizeAnimations.AutoSize = true;
            this.chkDoNotOptimizeAnimations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDoNotOptimizeAnimations.Location = new System.Drawing.Point(216, 279);
            this.chkDoNotOptimizeAnimations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDoNotOptimizeAnimations.Name = "chkDoNotOptimizeAnimations";
            this.chkDoNotOptimizeAnimations.Size = new System.Drawing.Size(191, 20);
            this.chkDoNotOptimizeAnimations.TabIndex = 18;
            this.chkDoNotOptimizeAnimations.Text = "Do Not Optimize Animations";
            this.chkDoNotOptimizeAnimations.UseVisualStyleBackColor = true;
            this.chkDoNotOptimizeAnimations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkAnimgroupExportNonAnimated
            // 
            this.chkAnimgroupExportNonAnimated.AutoSize = true;
            this.chkAnimgroupExportNonAnimated.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAnimgroupExportNonAnimated.Location = new System.Drawing.Point(19, 251);
            this.chkAnimgroupExportNonAnimated.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAnimgroupExportNonAnimated.Name = "chkAnimgroupExportNonAnimated";
            this.chkAnimgroupExportNonAnimated.Size = new System.Drawing.Size(311, 20);
            this.chkAnimgroupExportNonAnimated.TabIndex = 18;
            this.chkAnimgroupExportNonAnimated.Text = "(Animation Group) Export Non-Animated Objects";
            this.chkAnimgroupExportNonAnimated.UseVisualStyleBackColor = true;
            this.chkAnimgroupExportNonAnimated.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkMergeAO
            // 
            this.chkMergeAO.AutoSize = true;
            this.chkMergeAO.Checked = true;
            this.chkMergeAO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMergeAO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMergeAO.Location = new System.Drawing.Point(421, 221);
            this.chkMergeAO.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkMergeAO.Name = "chkMergeAO";
            this.chkMergeAO.Size = new System.Drawing.Size(116, 20);
            this.chkMergeAO.TabIndex = 17;
            this.chkMergeAO.Text = "Merge AO map";
            this.chkMergeAO.UseVisualStyleBackColor = true;
            this.chkMergeAO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // txtQuality
            // 
            this.txtQuality.Location = new System.Drawing.Point(936, 163);
            this.txtQuality.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txtQuality.Name = "txtQuality";
            this.txtQuality.Size = new System.Drawing.Size(56, 22);
            this.txtQuality.TabIndex = 9;
            this.txtQuality.Text = "100";
            this.txtQuality.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtQuality.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // labelQuality
            // 
            this.labelQuality.AutoSize = true;
            this.labelQuality.Location = new System.Drawing.Point(801, 168);
            this.labelQuality.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelQuality.Name = "labelQuality";
            this.labelQuality.Size = new System.Drawing.Size(97, 16);
            this.labelQuality.TabIndex = 8;
            this.labelQuality.Text = "Texture quality:";
            // 
            // chkExportMorphNormals
            // 
            this.chkExportMorphNormals.AutoSize = true;
            this.chkExportMorphNormals.Checked = true;
            this.chkExportMorphNormals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportMorphNormals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExportMorphNormals.Location = new System.Drawing.Point(216, 415);
            this.chkExportMorphNormals.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkExportMorphNormals.Name = "chkExportMorphNormals";
            this.chkExportMorphNormals.Size = new System.Drawing.Size(155, 20);
            this.chkExportMorphNormals.TabIndex = 16;
            this.chkExportMorphNormals.Text = "Export morph normals";
            this.chkExportMorphNormals.UseVisualStyleBackColor = true;
            this.chkExportMorphNormals.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkExportMorphTangents
            // 
            this.chkExportMorphTangents.AutoSize = true;
            this.chkExportMorphTangents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExportMorphTangents.Location = new System.Drawing.Point(19, 415);
            this.chkExportMorphTangents.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkExportMorphTangents.Name = "chkExportMorphTangents";
            this.chkExportMorphTangents.Size = new System.Drawing.Size(158, 20);
            this.chkExportMorphTangents.TabIndex = 16;
            this.chkExportMorphTangents.Text = "Export morph tangents";
            this.chkExportMorphTangents.UseVisualStyleBackColor = true;
            this.chkExportMorphTangents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkExportTangents
            // 
            this.chkExportTangents.AutoSize = true;
            this.chkExportTangents.Checked = true;
            this.chkExportTangents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportTangents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExportTangents.Location = new System.Drawing.Point(421, 165);
            this.chkExportTangents.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkExportTangents.Name = "chkExportTangents";
            this.chkExportTangents.Size = new System.Drawing.Size(117, 20);
            this.chkExportTangents.TabIndex = 16;
            this.chkExportTangents.Text = "Export tangents";
            this.chkExportTangents.UseVisualStyleBackColor = true;
            this.chkExportTangents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(817, 137);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Scale factor:";
            // 
            // txtScaleFactor
            // 
            this.txtScaleFactor.Location = new System.Drawing.Point(936, 132);
            this.txtScaleFactor.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txtScaleFactor.Name = "txtScaleFactor";
            this.txtScaleFactor.Size = new System.Drawing.Size(55, 22);
            this.txtScaleFactor.TabIndex = 7;
            this.txtScaleFactor.Text = "1";
            this.txtScaleFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtScaleFactor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output format:";
            // 
            // comboOutputFormat
            // 
            this.comboOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOutputFormat.Items.AddRange(new object[] {
            "gltf",
            "glb"});
            this.comboOutputFormat.Location = new System.Drawing.Point(129, 4);
            this.comboOutputFormat.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.comboOutputFormat.Name = "comboOutputFormat";
            this.comboOutputFormat.Size = new System.Drawing.Size(160, 24);
            this.comboOutputFormat.TabIndex = 5;
            this.comboOutputFormat.SelectedIndexChanged += new System.EventHandler(this.comboOutputFormat_SelectedIndexChanged);
            this.comboOutputFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkOnlySelected
            // 
            this.chkOnlySelected.AutoSize = true;
            this.chkOnlySelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkOnlySelected.Location = new System.Drawing.Point(216, 165);
            this.chkOnlySelected.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkOnlySelected.Name = "chkOnlySelected";
            this.chkOnlySelected.Size = new System.Drawing.Size(146, 20);
            this.chkOnlySelected.TabIndex = 13;
            this.chkOnlySelected.Text = "Export only selected";
            this.chkOnlySelected.UseVisualStyleBackColor = true;
            this.chkOnlySelected.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAutoSave.Location = new System.Drawing.Point(19, 193);
            this.chkAutoSave.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(130, 20);
            this.chkAutoSave.TabIndex = 15;
            this.chkAutoSave.Text = "Save 3ds Max file";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            this.chkAutoSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // chkHidden
            // 
            this.chkHidden.AutoSize = true;
            this.chkHidden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkHidden.Location = new System.Drawing.Point(19, 165);
            this.chkHidden.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkHidden.Name = "chkHidden";
            this.chkHidden.Size = new System.Drawing.Size(154, 20);
            this.chkHidden.TabIndex = 12;
            this.chkHidden.Text = "Export hidden objects";
            this.chkHidden.UseVisualStyleBackColor = true;
            this.chkHidden.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butClose.Location = new System.Drawing.Point(1117, 723);
            this.butClose.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(107, 28);
            this.butClose.TabIndex = 106;
            this.butClose.Text = "Close";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            this.butClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // saveOptionBtn
            // 
            this.saveOptionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveOptionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveOptionBtn.Location = new System.Drawing.Point(600, 723);
            this.saveOptionBtn.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.saveOptionBtn.Name = "saveOptionBtn";
            this.saveOptionBtn.Size = new System.Drawing.Size(115, 28);
            this.saveOptionBtn.TabIndex = 110;
            this.saveOptionBtn.Text = "Save Options";
            this.saveOptionBtn.UseVisualStyleBackColor = true;
            this.saveOptionBtn.Click += new System.EventHandler(this.saveOptionBtn_Click);
            // 
            // envFileDialog
            // 
            this.envFileDialog.DefaultExt = "dds";
            this.envFileDialog.Filter = "dds files|*.dds";
            this.envFileDialog.Title = "Select Environment";
            // 
            // butCopyToClipboard
            // 
            this.butCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butCopyToClipboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butCopyToClipboard.Location = new System.Drawing.Point(5, -1393);
            this.butCopyToClipboard.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.butCopyToClipboard.Name = "butCopyToClipboard";
            this.butCopyToClipboard.Size = new System.Drawing.Size(148, 28);
            this.butCopyToClipboard.TabIndex = 105;
            this.butCopyToClipboard.Text = "Copy To Clipboard";
            this.butCopyToClipboard.UseVisualStyleBackColor = true;
            this.butCopyToClipboard.Click += new System.EventHandler(this.butCopyToClipboard_Click);
            this.butCopyToClipboard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            // 
            // exporterTabControl
            // 
            this.exporterTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exporterTabControl.Controls.Add(this.exportOptionsTabPage);
            this.exporterTabControl.Controls.Add(this.logTabPage);
            this.exporterTabControl.Location = new System.Drawing.Point(3, 109);
            this.exporterTabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.exporterTabControl.Name = "exporterTabControl";
            this.exporterTabControl.SelectedIndex = 0;
            this.exporterTabControl.Size = new System.Drawing.Size(1233, 599);
            this.exporterTabControl.TabIndex = 112;
            // 
            // exportOptionsTabPage
            // 
            this.exportOptionsTabPage.Controls.Add(this.exportOptionsScrollPanel);
            this.exportOptionsTabPage.Location = new System.Drawing.Point(4, 25);
            this.exportOptionsTabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.exportOptionsTabPage.Name = "exportOptionsTabPage";
            this.exportOptionsTabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.exportOptionsTabPage.Size = new System.Drawing.Size(1225, 570);
            this.exportOptionsTabPage.TabIndex = 0;
            this.exportOptionsTabPage.Text = "Options";
            this.exportOptionsTabPage.UseVisualStyleBackColor = true;
            // 
            // exportOptionsScrollPanel
            // 
            this.exportOptionsScrollPanel.AutoScroll = true;
            this.exportOptionsScrollPanel.Controls.Add(this.comboOutputFormat);
            this.exportOptionsScrollPanel.Controls.Add(this.chkUseClone);
            this.exportOptionsScrollPanel.Controls.Add(this.chkTryReuseTexture);
            this.exportOptionsScrollPanel.Controls.Add(this.chkExportAnimationsOnly);
            this.exportOptionsScrollPanel.Controls.Add(this.chkExportTextures);
            this.exportOptionsScrollPanel.Controls.Add(this.label3);
            this.exportOptionsScrollPanel.Controls.Add(this.chkExportAnimations);
            this.exportOptionsScrollPanel.Controls.Add(this.label2);
            this.exportOptionsScrollPanel.Controls.Add(this.lblBakeAnimation);
            this.exportOptionsScrollPanel.Controls.Add(this.butModelBrowse);
            this.exportOptionsScrollPanel.Controls.Add(this.cmbBakeAnimationOptions);
            this.exportOptionsScrollPanel.Controls.Add(this.chkManifest);
            this.exportOptionsScrollPanel.Controls.Add(this.chkApplyPreprocessToScene);
            this.exportOptionsScrollPanel.Controls.Add(this.txtModelPath);
            this.exportOptionsScrollPanel.Controls.Add(this.chkMrgContainersAndXref);
            this.exportOptionsScrollPanel.Controls.Add(this.chkWriteTextures);
            this.exportOptionsScrollPanel.Controls.Add(this.chkUsePreExportProces);
            this.exportOptionsScrollPanel.Controls.Add(this.label1);
            this.exportOptionsScrollPanel.Controls.Add(this.chkFlatten);
            this.exportOptionsScrollPanel.Controls.Add(this.chkHidden);
            this.exportOptionsScrollPanel.Controls.Add(this.label5);
            this.exportOptionsScrollPanel.Controls.Add(this.chkAutoSave);
            this.exportOptionsScrollPanel.Controls.Add(this.label8);
            this.exportOptionsScrollPanel.Controls.Add(this.chkOnlySelected);
            this.exportOptionsScrollPanel.Controls.Add(this.txtEnvironmentName);
            this.exportOptionsScrollPanel.Controls.Add(this.txtScaleFactor);
            this.exportOptionsScrollPanel.Controls.Add(this.label6);
            this.exportOptionsScrollPanel.Controls.Add(this.label4);
            this.exportOptionsScrollPanel.Controls.Add(this.chkExportTangents);
            this.exportOptionsScrollPanel.Controls.Add(this.btnEnvBrowse);
            this.exportOptionsScrollPanel.Controls.Add(this.chkExportMorphTangents);
            this.exportOptionsScrollPanel.Controls.Add(this.chkNoAutoLight);
            this.exportOptionsScrollPanel.Controls.Add(this.chkExportMorphNormals);
            this.exportOptionsScrollPanel.Controls.Add(this.textureLabel);
            this.exportOptionsScrollPanel.Controls.Add(this.labelQuality);
            this.exportOptionsScrollPanel.Controls.Add(this.txtTexturesPath);
            this.exportOptionsScrollPanel.Controls.Add(this.txtQuality);
            this.exportOptionsScrollPanel.Controls.Add(this.btnTxtBrowse);
            this.exportOptionsScrollPanel.Controls.Add(this.chkMergeAO);
            this.exportOptionsScrollPanel.Controls.Add(this.chkExportMaterials);
            this.exportOptionsScrollPanel.Controls.Add(this.chkKHRMaterialsUnlit);
            this.exportOptionsScrollPanel.Controls.Add(this.chkAnimgroupExportNonAnimated);
            this.exportOptionsScrollPanel.Controls.Add(this.chkKHRTextureTransform);
            this.exportOptionsScrollPanel.Controls.Add(this.chkDoNotOptimizeAnimations);
            this.exportOptionsScrollPanel.Controls.Add(this.chkKHRLightsPunctual);
            this.exportOptionsScrollPanel.Controls.Add(this.chkOverwriteTextures);
            this.exportOptionsScrollPanel.Location = new System.Drawing.Point(4, 4);
            this.exportOptionsScrollPanel.Margin = new System.Windows.Forms.Padding(0);
            this.exportOptionsScrollPanel.Name = "exportOptionsScrollPanel";
            this.exportOptionsScrollPanel.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.exportOptionsScrollPanel.Size = new System.Drawing.Size(1215, 561);
            this.exportOptionsScrollPanel.TabIndex = 44;
            // 
            // chkUseClone
            // 
            this.chkUseClone.AutoSize = true;
            this.chkUseClone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkUseClone.Location = new System.Drawing.Point(609, 193);
            this.chkUseClone.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkUseClone.Name = "chkUseClone";
            this.chkUseClone.Size = new System.Drawing.Size(174, 20);
            this.chkUseClone.TabIndex = 45;
            this.chkUseClone.Text = "Use clone (experimental)";
            this.chkUseClone.UseVisualStyleBackColor = true;
            // 
            // chkTryReuseTexture
            // 
            this.chkTryReuseTexture.AutoSize = true;
            this.chkTryReuseTexture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkTryReuseTexture.Location = new System.Drawing.Point(609, 165);
            this.chkTryReuseTexture.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chkTryReuseTexture.Name = "chkTryReuseTexture";
            this.chkTryReuseTexture.Size = new System.Drawing.Size(124, 20);
            this.chkTryReuseTexture.TabIndex = 44;
            this.chkTryReuseTexture.Text = "Try reuse texture";
            this.chkTryReuseTexture.UseVisualStyleBackColor = true;
            // 
            // logTabPage
            // 
            this.logTabPage.Controls.Add(this.logTreeView);
            this.logTabPage.Controls.Add(this.butCopyToClipboard);
            this.logTabPage.Location = new System.Drawing.Point(4, 25);
            this.logTabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logTabPage.Size = new System.Drawing.Size(1225, 570);
            this.logTabPage.TabIndex = 1;
            this.logTabPage.Text = "Log";
            this.logTabPage.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Max2Babylon.Properties.Resources.MaxExporter;
            this.pictureBox1.Location = new System.Drawing.Point(8, 4);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(684, 99);
            this.pictureBox1.TabIndex = 113;
            this.pictureBox1.TabStop = false;
            // 
            // ExporterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 765);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.exporterTabControl);
            this.Controls.Add(this.saveOptionBtn);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.butExport);
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.MinimumSize = new System.Drawing.Size(1117, 451);
            this.Name = "ExporterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Babylon.js - Export scene to babylon or glTF format";
            this.Activated += new System.EventHandler(this.ExporterForm_Activated);
            this.Deactivate += new System.EventHandler(this.ExporterForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExporterForm_FormClosed);
            this.Load += new System.EventHandler(this.ExporterForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExporterForm_KeyDown);
            this.exporterTabControl.ResumeLayout(false);
            this.exportOptionsTabPage.ResumeLayout(false);
            this.exportOptionsScrollPanel.ResumeLayout(false);
            this.exportOptionsScrollPanel.PerformLayout();
            this.logTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtModelPath;
        private System.Windows.Forms.Button butModelBrowse;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TreeView logTreeView;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox chkManifest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkHidden;
        private System.Windows.Forms.CheckBox chkAutoSave;
        //private System.Windows.Forms.Button butExportAndRun;
        private System.Windows.Forms.CheckBox chkOnlySelected;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.ComboBox comboOutputFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtScaleFactor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkExportTangents;
        private System.Windows.Forms.Label labelQuality;
        private System.Windows.Forms.TextBox txtQuality;
        private System.Windows.Forms.CheckBox chkMergeAO;
        //private System.Windows.Forms.CheckBox chkDracoCompression;
        //private System.Windows.Forms.ToolTip toolTipDracoCompression;
        private System.Windows.Forms.CheckBox chkOverwriteTextures;
        //private System.Windows.Forms.Button butMultiExport;
        private System.Windows.Forms.CheckBox chkKHRLightsPunctual;
        private System.Windows.Forms.CheckBox chkKHRTextureTransform;
        private System.Windows.Forms.CheckBox chkKHRMaterialsUnlit;
        private System.Windows.Forms.CheckBox chkExportMaterials;
        private System.Windows.Forms.Button saveOptionBtn;
        private System.Windows.Forms.Label textureLabel;
        private System.Windows.Forms.RichTextBox txtTexturesPath;
        private System.Windows.Forms.Button btnTxtBrowse;
        private System.Windows.Forms.Label label5;
        //private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox txtEnvironmentName;
        private System.Windows.Forms.Label label6;
        //private System.Windows.Forms.CheckBox chkFullPBR;
        private System.Windows.Forms.Button btnEnvBrowse;
        private System.Windows.Forms.CheckBox chkNoAutoLight;
        private System.Windows.Forms.CheckBox chkWriteTextures;
        private System.Windows.Forms.OpenFileDialog envFileDialog;
        private System.Windows.Forms.CheckBox chkAnimgroupExportNonAnimated;
        private System.Windows.Forms.CheckBox chkDoNotOptimizeAnimations;
        private System.Windows.Forms.CheckBox chkExportMorphTangents;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkExportMorphNormals;
        private System.Windows.Forms.CheckBox chkFlatten;
        private System.Windows.Forms.CheckBox chkUsePreExportProces;
        private System.Windows.Forms.CheckBox chkMrgContainersAndXref;
        private System.Windows.Forms.CheckBox chkApplyPreprocessToScene;
        private System.Windows.Forms.Label lblBakeAnimation;
        private System.Windows.Forms.ComboBox cmbBakeAnimationOptions;
        private System.Windows.Forms.CheckBox chkExportAnimationsOnly;
        private System.Windows.Forms.CheckBox chkExportAnimations;
        private System.Windows.Forms.Button butCopyToClipboard;
        private System.Windows.Forms.CheckBox chkExportTextures;
        private System.Windows.Forms.TabControl exporterTabControl;
        private System.Windows.Forms.TabPage exportOptionsTabPage;
        private System.Windows.Forms.TabPage logTabPage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel exportOptionsScrollPanel;
        private System.Windows.Forms.CheckBox chkTryReuseTexture;

        private System.Windows.Forms.CheckBox chkUseClone;

        //private System.Windows.Forms.TabPage advancedTabPage;
        //private System.Windows.Forms.GroupBox dracoGroupBox;
        //private Utilities.DracoUserControl dracoUserControl;
    }
}
