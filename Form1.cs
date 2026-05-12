using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenTK.Mathematics;
using OpenTKProject;

namespace WinFormsOpenTK
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer _fpsTimer;
        private System.Windows.Forms.Timer _renderTimer;
        private System.Windows.Forms.Timer _inputTimer;

        private Renderer _renderer;
        private int _frameCount;
        private float _fps;
        private DateTime _lastFPSTime = DateTime.Now;
        private DataLoader dataLoader;

        private string _selectedModelPath = "";
        private ModelTextures _selectedTexturePath;
        private string _selectedModelFormat;

        private SceneGL sceneGL;
        private GameScene gameScene;
        private CameraModule cameraModule;

        private bool isPhysicsEnabled;
        private bool isPhysicsKinematic;
        private bool isCollide;

        public Form1()
        {
            InitializeComponent();
            InitializeScenePipeline();
            SetupTimers();
            StartTimers();
            LoadData();

            gameScene.SetMainLight();
        }

        #region InitMethods

        private void InitializeScenePipeline()
        {
            gameScene = new GameScene();
            _renderer = gameScene.InitRender();
            sceneGL = new SceneGL(ref _glControl, ref _renderer);
            sceneGL.OpenGLInitialized += OnOpenGLInitialized;

            cameraModule = gameScene.GetEngineCameraModule();
            sceneGL.SetCamera(cameraModule);
            sceneGL.InitializeOpenGL();
        }

        private void OnOpenGLInitialized()
        {
            gameScene.SetSkyBox();
            gameScene.StartObjects();
        }

        private void StartTimers()
        {
            _inputTimer.Start();
            _renderTimer.Start();
            _fpsTimer.Start();
        }

        private void SetupTimers()
        {
            _fpsTimer = new System.Windows.Forms.Timer { Interval = 1 };
            _fpsTimer.Tick += (s, e) =>
            {
                fPSCounter.Text = $"FPS: {_fps:F1}";
                _frameCount = 0;
            };

            _inputTimer = new System.Windows.Forms.Timer { Interval = 5 };
            _inputTimer.Tick += (s, e) =>
            {
                if (!_glControl.IsDisposed)
                {
                    float deltaTime = _inputTimer.Interval / 1000.0f;
                    gameScene.UpdatePhysics(deltaTime);
                }
            };

            _renderTimer = new System.Windows.Forms.Timer { Interval = 8 };
            _renderTimer.Tick += (s, e) =>
            {
                if (_glControl.IsDisposed) return;

                try
                {
                    _glControl.MakeCurrent();

                    gameScene.UpdateObjects(0.008f);

                    _renderer.Render(gameScene.GetRenderList(), new Vector2i(_glControl.Width, _glControl.Height));
                    _glControl.SwapBuffers();

                    _frameCount++;
                    _fps = (float)(_frameCount / (DateTime.Now - _lastFPSTime).TotalSeconds);
                    _lastFPSTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Render error: {ex.Message}");
                }
            };
        }

        private void LoadData()
        {
            try
            {
                UpdateComboBox();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region EventArgsMethods

        private void DeleteModels_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 &&
                listBox1.SelectedIndex < gameScene.GetSceneObjectsCount())
            {
                int index = listBox1.SelectedIndex;

                gameScene.DeleteSceneObjectByIndex(index);

                listBox1.Items.RemoveAt(index);

                listBox1.Enabled = gameScene.GetSceneObjectsCount() > 0;
            }
        }

        private void BtnAddObject_Click(object sender, EventArgs e)
        {
            try
            {
                Vector3 position = new Vector3(float.Parse(xCoord.Text),
                        float.Parse(yCoord.Text),
                        float.Parse(zCoord.Text));

                Vector3 scale = new Vector3(float.Parse(xScale.Text),
                        float.Parse(yScale.Text),
                        float.Parse(zScale.Text));

                var Transform = new TransformObject(position, scale);
                var model = gameScene.CreateNewModel(Transform, _selectedModelPath, _selectedTexturePath, _selectedModelFormat);
                model = SetupObject(model);

                string fileName = Path.GetFileNameWithoutExtension(_selectedModelPath);
                listBox1.Items.Add($"{fileName}({ position.X:F1}, { position.Y:F1}, { position.Z:F1})");

                DeleteModelsBtn.Enabled = true;

                listBox1.Enabled = gameScene.GetSceneObjectsCount() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding object: {ex.Message}\n\nStack trace: {ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SceneObject SetupObject(SceneObject ScObj)
        {
            if (isPhysicsEnabled)
            {
                var phys = ModuleInitializer.AddPhysicsModule(ScObj, gameScene.GetPhysics(), isPhysicsKinematic);
                ScObj.AddModule(phys);
            }
            if (isCollide)
            {
                Vector3 colliderSize = new Vector3(2f, 2f, 2f);

                var coll = ModuleInitializer.AddCollisionModule(ScObj, gameScene.GetPhysics(), colliderSize);
                ScObj.AddModule(coll);
            }
            ScObj.Start();

            return ScObj;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = dataLoader.GetLoadedData();

            if (data != null)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].PresetName == comboBox1.SelectedItem.ToString())
                    {
                        _selectedModelPath = data[i].ModelPath;
                        _selectedTexturePath.TexturePath = data[i].TexturePath;
                        _selectedTexturePath.NormalMapPath = data[i].NormalMapPath;
                        _selectedTexturePath.MetallicMapPath = data[i].MetallicMapPath;
                        _selectedTexturePath.RoughnessMapPath = data[i].RoughnessMapPath;
                        _selectedModelFormat = data[i].ModelFormat;
                    }
                }
            }
        }

        private void isPhysicsAdded_Check(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            isPhysicsEnabled = check.Checked;
        }

        private void isObjectKinematic_Check(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            isPhysicsKinematic = check.Checked;
        }

        private void isCollision_Check(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            isCollide = check.Checked;
        }

        private void BindCamToSelect_Check(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 &&
                listBox1.SelectedIndex < gameScene.GetSceneObjectsCount())
            {
                int index = listBox1.SelectedIndex;

                gameScene.SetEngineCameraOrbitMode(index);
            }
        }

        private void UnbindCam_Check(object sender, EventArgs e)
        {
            gameScene.GetEngineCameraModule().SetFreeMode();
            listBox1.ClearSelected();
        }

        #endregion


        private Vector3 CalculateModelBounds(Model model)
        {
            if (model?.VModel?.Vertices == null)
                return new Vector3(1f, 1f, 1f);

            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            var vertices = model.VModel.Vertices;
            for (int i = 0; i < vertices.Count; i += 8)
            {
                if (i + 2 >= vertices.Count) break;

                float x = vertices[i];
                float y = vertices[i + 1];
                float z = vertices[i + 2];

                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);
                minZ = Math.Min(minZ, z);
                maxZ = Math.Max(maxZ, z);
            }

            return new Vector3(
                maxX - minX,
                maxY - minY,
                maxZ - minZ
            );
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _inputTimer?.Stop();
            _renderTimer?.Stop();
            _fpsTimer?.Stop();

            gameScene.Dispose();

            _glControl?.Dispose();
        }

        private void Coord_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            string newText = textBox.Text.Substring(0, textBox.SelectionStart) +
                e.KeyChar +
                textBox.Text.Substring(textBox.SelectionStart + textBox.SelectionLength);

            Regex reg = new Regex(@"^-?\d*\.?\d*$");

            if (!reg.IsMatch(newText))
                e.Handled = true;
        }

        private void LightIntense_ValueChanged(object sender, EventArgs e)
        {
            gameScene.SetGlobalLightIntensity((float)LightIntense.Value);
        }

        private void SaveDB_Click(object sender, EventArgs e)
        {
            try
            {
                dataLoader.SaveChanges();
                UpdateComboBox();
                MessageBox.Show("Saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteDBData_Click(object sender, EventArgs e)
        {
            if (dataLoader.DeleteSelected(dataGridViewModels))
                MessageBox.Show("Deleted!");
            else
                MessageBox.Show("No selected data.");
        }

        private void UpdateComboBox()
        {
            dataLoader = new DataLoader();
            dataLoader.LoadData();
            dataLoader.LoadToDataGridView(dataGridViewModels);

            var data = dataLoader.GetLoadedData();
            List<string> names = new List<string>();

            for (int i = 0; i < data.Count; i++)
            {
                names.Add(data[i].PresetName.ToString());
            }

            comboBox1.DataSource = names;
        }
    }
}
