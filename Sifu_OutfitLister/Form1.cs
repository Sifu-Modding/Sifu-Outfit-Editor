using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;
using System.Text.RegularExpressions;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.FieldTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;

namespace Sifu_OutfitLister
{
    public partial class Frm_Main : Form
    {

        List<Outfit> outfitList = new List<Outfit>();

        public Frm_Main()
        {
            InitializeComponent();

            LsbOutfits.DataSource = new BindingSource(outfitList, null);
            LsbOutfits.DisplayMember = "Name";
            LsbOutfits.ValueMember = "Description";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string folder = dialog.FileName;

                string dirBlueprints = folder + "\\Sifu\\Content\\Blueprints\\";
                string dirUI = folder + "\\Sifu\\Content\\UI\\Blueprints\\Menus\\Outfits\\";

                // If directory does not exist, create it
                if (!Directory.Exists(dirBlueprints))
                {
                    Directory.CreateDirectory(dirBlueprints);
                }

                // If directory does not exist, create it
                if (!Directory.Exists(dirUI))
                {
                    Directory.CreateDirectory(dirUI);
                }

                AddOutfits(folder);
                AddMenuButtons(folder);
            }


        }

        private void AddNewOutfitInFightingPlayer(UAsset FightingPlayerAsset, ArrayPropertyData M_Outfits, Outfit outfit)
        {
            var outf = M_Outfits.Value[0] as StructPropertyData;
            StructPropertyData newOutfit = new StructPropertyData();
            if (outf is not null)
            {

                newOutfit.Name = new UAssetAPI.UnrealTypes.FName(FightingPlayerAsset, "m_Outfits");
                newOutfit.Value = new List<PropertyData>();

                foreach (var item in outf.Value)
                {
                    newOutfit.Value.Add(item);
                }

                SoftObjectPropertyData softObjectPropertyData = new SoftObjectPropertyData();
                softObjectPropertyData.Name = new UAssetAPI.UnrealTypes.FName(FightingPlayerAsset, "m_Mesh");
                FSoftObjectPath softObj = new FSoftObjectPath();

                softObj.AssetPathName = new UAssetAPI.UnrealTypes.FName(FightingPlayerAsset, outfit.Path);
                softObjectPropertyData.Value = softObj;

                newOutfit.Value[0] = softObjectPropertyData;

                var data = M_Outfits.Value.Append(newOutfit);
                M_Outfits.Value = data.ToArray();
            }
        }

        private void AddOutfits(string folder)
        {
            string exePath = Path.GetDirectoryName(Application.ExecutablePath);
            var FightingPlayerAsset = new UAsset(exePath + "\\BP_FightingPlayer.uasset", UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_26);

            var export = FightingPlayerAsset.Exports.Where(e => e.ObjectName.ToString().Equals("PlayerFighting_GEN_VARIABLE")).First() as NormalExport;

            if (export is not null)
            {
                foreach (PropertyData ex in export.Data)
                {

                    if (ex.Name.ToString().Equals("m_ByGenderData"))
                    {
                        var propertyGenderData = ex as StructPropertyData;
                        var M_Outfits = propertyGenderData?.Value[0] as ArrayPropertyData;

                        if (M_Outfits is not null)
                        {
                            foreach (Outfit outfit in outfitList)
                            {
                                AddNewOutfitInFightingPlayer(FightingPlayerAsset, M_Outfits, outfit);
                            }
                        }
                    }
                }

                FightingPlayerAsset.Write(folder + "\\Sifu\\Content\\Blueprints\\BP_FightingPlayer.uasset");
            }
        }

        private void AddMenuButtons(string folder)
        {
            string exePath = Path.GetDirectoryName(Application.ExecutablePath);
            var MenuOutfit = new UAsset(exePath + "\\BP_Menu_Outfits.uasset", UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_26);

            var scrollBox = MenuOutfit.Exports.Where(e => e.ObjectName.ToString().Equals("OutfitsScroll")).First() as NormalExport;
            var BtnTemplate = MenuOutfit.Exports.Where(e => e.ObjectName.ToString().Equals("Btn_DefaultOutfit")).First() as NormalExport;
            var ScrollBoxSlot = MenuOutfit.Exports.Where(e => e.ObjectName.ToString().Equals("ScrollBoxSlot_0")).First() as NormalExport;
            var BPMenuOutfits = MenuOutfit.Exports.Where(e => e.ObjectName.ToString().Equals("BP_Menu_Outfits_C")).First() as ClassExport;
            int startingSlot = 12;
            foreach (Outfit outfit in outfitList)
            {
                string btnName = "Btn_" + removeSpecialChars(outfit.Name.Replace(" ", ""));
                AddButton(MenuOutfit, BtnTemplate, outfit, btnName, startingSlot);
                AddSlot(MenuOutfit, ScrollBoxSlot, btnName);
                AddSlotToArray(MenuOutfit, scrollBox, startingSlot);
                FixWidgetNavigation(MenuOutfit);
                AddButtonBindings(MenuOutfit, BPMenuOutfits, btnName);
                AddLoadedProperties(MenuOutfit, BPMenuOutfits, BtnTemplate, btnName);
                startingSlot++;
            }

            MenuOutfit.Write(folder + "\\Sifu\\Content\\UI\\Blueprints\\Menus\\Outfits\\BP_Menu_Outfits.uasset");
        }


        private void AddButton(UAsset MenuOutfit, NormalExport BtnTemplate, Outfit outfit, string btnName, int startingSlot)
        {
            NormalExport BtnOutfit = new NormalExport();
            BtnOutfit.ClassIndex = BtnTemplate.ClassIndex;
            BtnOutfit.TemplateIndex = BtnTemplate.TemplateIndex;
            BtnOutfit.ObjectFlags = BtnTemplate.ObjectFlags;
            BtnOutfit.CreateBeforeCreateDependencies = BtnTemplate.CreateBeforeCreateDependencies;
            BtnOutfit.CreateBeforeSerializationDependencies = BtnTemplate.CreateBeforeSerializationDependencies;
            BtnOutfit.SerializationBeforeCreateDependencies = BtnTemplate.SerializationBeforeCreateDependencies;
            BtnOutfit.Extras = BtnTemplate.Extras;
            BtnOutfit.Data = new List<PropertyData>();
            foreach (var item in BtnTemplate.Data)
            {
                BtnOutfit.Data.Add(item);
            }

            TextPropertyData Title = new TextPropertyData();
            Title.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "Title");
            Title.HistoryType = TextHistoryType.Base;
            Title.CultureInvariantString = new UAssetAPI.UnrealTypes.FString(outfit.Name);
            Title.Value = new UAssetAPI.UnrealTypes.FString("D7F315B340D9DF9644B3D7AFC5C9F449");

            BtnOutfit.Data[0] = Title;

            TextPropertyData Description = new TextPropertyData();
            Description.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "Description");
            Description.HistoryType = TextHistoryType.Base;
            Description.CultureInvariantString = new UAssetAPI.UnrealTypes.FString(outfit.Description);
            Description.Value = new UAssetAPI.UnrealTypes.FString("BB8377C64D936FD2EAE81C9039FCFB4E");

            BtnOutfit.Data[2] = Description;

            ObjectPropertyData Slot = new();
            Slot.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "Slot");
            Slot.Value = new UAssetAPI.UnrealTypes.FPackageIndex(MenuOutfit.Exports.Count + 2);
            BtnOutfit.Data[4] = Slot;

            IntPropertyData iOutfitIndex = new();
            iOutfitIndex.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "iOutfitIndex");
            iOutfitIndex.Value = startingSlot;

            BtnOutfit.Data[5] = iOutfitIndex;
            MenuOutfit.Exports.Add(BtnOutfit);
            BtnOutfit.ObjectName = new UAssetAPI.UnrealTypes.FName(MenuOutfit, btnName);

        }

        private void AddSlot(UAsset MenuOutfit, NormalExport ScrollBoxSlot, string btnName)
        {
            NormalExport ScrollBoxSlotTest = new NormalExport();

            ScrollBoxSlotTest.ClassIndex = ScrollBoxSlot.ClassIndex;
            ScrollBoxSlotTest.TemplateIndex = ScrollBoxSlot.TemplateIndex;
            ScrollBoxSlotTest.ObjectFlags = ScrollBoxSlot.ObjectFlags;
            ScrollBoxSlotTest.CreateBeforeCreateDependencies = ScrollBoxSlot.CreateBeforeCreateDependencies;
            ScrollBoxSlotTest.CreateBeforeSerializationDependencies = ScrollBoxSlot.CreateBeforeSerializationDependencies;
            ScrollBoxSlotTest.SerializationBeforeCreateDependencies = ScrollBoxSlot.SerializationBeforeCreateDependencies;
            ScrollBoxSlotTest.Extras = ScrollBoxSlot.Extras;
            ScrollBoxSlotTest.Data = new List<PropertyData>();
            foreach (var item in ScrollBoxSlot.Data)
            {
                ScrollBoxSlotTest.Data.Add(item);
            }

            ObjectPropertyData Content = new();
            Content.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "Content");
            Content.Value = new UAssetAPI.UnrealTypes.FPackageIndex(MenuOutfit.Exports.Count);

            ScrollBoxSlotTest.Data[2] = Content;
            MenuOutfit.Exports.Add(ScrollBoxSlotTest);
            ScrollBoxSlotTest.ObjectName = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "ScrollBoxSlot_" + btnName);
        }

        private void AddSlotToArray(UAsset MenuOutfit, NormalExport scrollBox, int startingSlot)
        {
            var array = scrollBox.Data[5] as ArrayPropertyData;
            ObjectPropertyData Slot = new();
            Slot.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, startingSlot.ToString());
            Slot.Value = new UAssetAPI.UnrealTypes.FPackageIndex(MenuOutfit.Exports.Count);
            var newArray = array.Value.Append(Slot);
            array.Value = newArray.ToArray();
        }

        private void FixWidgetNavigation(UAsset MenuOutfit)
        {
            var WidgetNavigations = MenuOutfit.Exports.Where(e => e.ObjectName.ToString().Equals("WidgetNavigation_0"));

            foreach (var wn in WidgetNavigations)
            {
                var WidgetNavigation = wn as NormalExport;

                foreach (var widget in WidgetNavigation.Data)
                {
                    var widNav = widget as StructPropertyData;
                    if (widNav != null)
                    {
                        var loopList = widNav.Value[1] as NamePropertyData;
                        loopList.Value = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "Btn_Wude");
                    }
                }
            }
        }

        private void AddButtonBindings(UAsset MenuOutfit, ClassExport BPMenuOutfits, string btnName)
        {
            var bindings = BPMenuOutfits.Data[2] as ArrayPropertyData;

            var bindOnFocus = bindings.Value[0] as StructPropertyData;
            var bindOnClick = bindings.Value[3] as StructPropertyData;

            StructPropertyData newBindOnFocus = new StructPropertyData();
            newBindOnFocus.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "Bindings");
            newBindOnFocus.Value = new List<PropertyData>();

            foreach (var item in bindOnFocus.Value)
            {
                newBindOnFocus.Value.Add(item);
            }

            StructPropertyData newBindOnClick = new StructPropertyData();
            newBindOnClick.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "Bindings");
            newBindOnClick.Value = new List<PropertyData>();

            foreach (var item in bindOnClick.Value)
            {
                newBindOnClick.Value.Add(item);
            }


            var objectName = newBindOnFocus.Value[0] as StrPropertyData;
            objectName.Value = new UAssetAPI.UnrealTypes.FString(btnName);

            var objName = newBindOnClick.Value[0] as StrPropertyData;
            objName.Value = new UAssetAPI.UnrealTypes.FString(btnName);

            var newBindings = bindings.Value.Append(newBindOnFocus);
            var newBindings2 = newBindings.Append(newBindOnClick);
            bindings.Value = newBindings2.ToArray();
        }

        private void AddLoadedProperties(UAsset MenuOutfit, ClassExport BPMenuOutfits, NormalExport BtnTemplate, string btnName)
        {
            var testing = new FObjectProperty();
            testing.Name = new UAssetAPI.UnrealTypes.FName(MenuOutfit, btnName);
            testing.PropertyFlags = UAssetAPI.UnrealTypes.EPropertyFlags.CPF_BlueprintVisible | UAssetAPI.UnrealTypes.EPropertyFlags.CPF_ExportObject | UAssetAPI.UnrealTypes.EPropertyFlags.CPF_InstancedReference | UAssetAPI.UnrealTypes.EPropertyFlags.CPF_RepSkip | UAssetAPI.UnrealTypes.EPropertyFlags.CPF_InstancedReference | UAssetAPI.UnrealTypes.EPropertyFlags.CPF_PersistentInstance;
            testing.ElementSize = 8;
            testing.RepIndex = 0;
            testing.SerializedType = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "ObjectProperty");
            testing.PropertyClass = BtnTemplate.ClassIndex;
            testing.ArrayDim = 0;
            testing.Flags = UAssetAPI.UnrealTypes.EObjectFlags.RF_Public | UAssetAPI.UnrealTypes.EObjectFlags.RF_LoadCompleted;
            testing.RepNotifyFunc = new UAssetAPI.UnrealTypes.FName(MenuOutfit, "None");
            testing.BlueprintReplicationCondition = ELifetimeCondition.COND_None;

            var LP = BPMenuOutfits.LoadedProperties.Append(testing);
            BPMenuOutfits.LoadedProperties = LP.ToArray();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbName.Text) || string.IsNullOrWhiteSpace(txbDescription.Text) || string.IsNullOrWhiteSpace(txbSKPath.Text))
            {
                MessageBox.Show("Fill in all the fields (Name, Desc, SK Path)");
            }
            else
            {
                Outfit outfit = new Outfit(txbName.Text, txbDescription.Text, txbSKPath.Text);

                outfitList.Add(outfit);
                LsbOutfits.DataSource = null;
                LsbOutfits.DataSource = new BindingSource(outfitList, null);
                LsbOutfits.DisplayMember = "Name";
                LsbOutfits.ValueMember = "Description";

                if (!btnGenerate.Enabled) btnGenerate.Enabled = true;

                txbName.Text = "";
                txbDescription.Text = "";
                txbSKPath.Text = "";
            }
        }

        public static string removeSpecialChars(string text)
        {
            return Regex.Replace(text, "[^0-9A-Za-z _-]", "");
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            outfitList.RemoveAt(LsbOutfits.SelectedIndex);
            LsbOutfits.DataSource = null;
            LsbOutfits.DataSource = new BindingSource(outfitList, null);
            LsbOutfits.DisplayMember = "Name";
            LsbOutfits.ValueMember = "Description";

            if (outfitList.Count <= 0) btnGenerate.Enabled = false;
        }
    }
}