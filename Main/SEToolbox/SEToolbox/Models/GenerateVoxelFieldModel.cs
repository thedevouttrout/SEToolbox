﻿namespace SEToolbox.Models
{
    using Sandbox.CommonLib.ObjectBuilders;
    using SEToolbox.Interop;
    using SEToolbox.Support;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    public class GenerateVoxelFieldModel : BaseModel
    {
        #region Fields

        private int _minimumRange;
        private int _maximumRange;
        private MyPositionAndOrientation _characterPosition;
        private ObservableCollection<GenerateVoxelDetailModel> _stockVoxelFileList;
        private readonly ObservableCollection<MaterialSelectionModel> _materialsCollection;
        private ObservableCollection<GenerateVoxelModel> _voxelCollection;
        private List<int> _percentList;

        #endregion

        #region ctor

        public GenerateVoxelFieldModel()
        {
            this._stockVoxelFileList = new ObservableCollection<GenerateVoxelDetailModel>();
            this._materialsCollection = new ObservableCollection<MaterialSelectionModel>();
            this._voxelCollection = new ObservableCollection<GenerateVoxelModel>();
            this._percentList = new List<int>();
        }

        #endregion

        #region Properties

        public ObservableCollection<GenerateVoxelModel> VoxelCollection
        {
            get
            {
                return this._voxelCollection;
            }

            set
            {
                if (value != this._voxelCollection)
                {
                    this._voxelCollection = value;
                    this.RaisePropertyChanged(() => VoxelCollection);
                }
            }
        }

        public int MinimumRange
        {
            get
            {
                return this._minimumRange;
            }

            set
            {
                if (value != this._minimumRange)
                {
                    this._minimumRange = value;
                    this.RaisePropertyChanged(() => MinimumRange);
                }
            }
        }

        public int MaximumRange
        {
            get
            {
                return this._maximumRange;
            }

            set
            {
                if (value != this._maximumRange)
                {
                    this._maximumRange = value;
                    this.RaisePropertyChanged(() => MaximumRange);
                }
            }
        }

        public MyPositionAndOrientation CharacterPosition
        {
            get
            {
                return this._characterPosition;
            }

            set
            {
                //if (value != this.characterPosition) // Unable to check for equivilence, without long statement. And, mostly uncessary.
                this._characterPosition = value;
                this.RaisePropertyChanged(() => CharacterPosition);
            }
        }

        public ObservableCollection<GenerateVoxelDetailModel> StockVoxelFileList
        {
            get
            {
                return this._stockVoxelFileList;
            }

            set
            {
                if (value != this._stockVoxelFileList)
                {
                    this._stockVoxelFileList = value;
                    this.RaisePropertyChanged(() => StockVoxelFileList);
                }
            }
        }

        public ObservableCollection<MaterialSelectionModel> MaterialsCollection
        {
            get
            {
                return this._materialsCollection;
            }
        }

        public List<int> PercentList
        {
            get
            {
                return this._percentList;
            }
        }

        #endregion

        #region methods

        public void Load(MyPositionAndOrientation characterPosition)
        {
            this.CharacterPosition = characterPosition;

            this.MaterialsCollection.Clear();
            foreach (var material in SpaceEngineersAPI.GetMaterialList())
            {
                this.MaterialsCollection.Add(new MaterialSelectionModel() { Value = material.Name, DisplayName = material.Name });
            }

            var files = Directory.GetFiles(Path.Combine(ToolboxUpdater.GetApplicationFilePath(), @"Content\VoxelMaps"), "*.vox");

            this.StockVoxelFileList.Clear();
            this.StockVoxelFileList.Add(new GenerateVoxelDetailModel()
            {
                Name = "None"
            });
            foreach (var file in files)
            {
                this.StockVoxelFileList.Add(new GenerateVoxelDetailModel()
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    SourceFilename = file,
                });
            }
        
            // Set up a default start.
            this.VoxelCollection = new ObservableCollection<GenerateVoxelModel>();
            for (var i = 0; i < 1; i++)
            {
                this.VoxelCollection.Add(this.NewDefaultVoxel(i + 1));
            }

            for (var i = 0; i < 100; i++)
            {
                this.PercentList.Add(i);
            }

            this.MinimumRange = 400;
            this.MaximumRange = 800;
        }

        public GenerateVoxelModel NewDefaultVoxel(int index)
        {
            return new GenerateVoxelModel()
                {
                    Index = index,
                    VoxelFile = this.StockVoxelFileList[1],
                    MainMaterial = this.MaterialsCollection[0],
                    SecondMaterial = this.MaterialsCollection[0],
                    ThirdMaterial = this.MaterialsCollection[0],
                    ForthMaterial = this.MaterialsCollection[0],
                };
        }

        #endregion
    }
}