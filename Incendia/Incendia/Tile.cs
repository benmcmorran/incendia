using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incendia
{
    public enum Layer1TileType
    {
        Carpet1,
        Carpet2,
        Tile1,
        Tile2,
        WoodFloor,
        GraniteWall,
        WoodWall,
        Outside
    }

    public enum Layer2TileType
    {
        Desk,
        Sofa,
        Plant,
        OpenHorizontalDoor,
        OpenVerticalDoor,
        ClosedHorizontalDoor,
        ClosedVerticalDoor,
        TrashCan,
        Newspaper,
        Flammables,
        Empty
    }

    public enum Layer3TileType
    {
        Computer,
        Blotter,
        Plant,
        Laptop,
        Empty
    }

    public class Tile
    {
        public float Flammability { get; private set; }
        public int Material { get; private set; }
        public FireState State { get; set; }
        public string _texture1;
        public string _texture2;
        public string _texture3;
        public bool _solid;
        public bool EXPLODES;
        public bool Solid { get { return _solid && State != FireState.Burnt; } }
        public bool Outside {get; set;}
        public string Texture1 { get { return _texture1 + TextureEnding(); } }
        public string Texture2 { get { return _texture2 + TextureEnding(); } }
        public string Texture3 { get { return _texture3 + TextureEnding(); } }

        public Tile(float flammability, int material,
            string texture1, string texture2, string texture3,
            FireState state, bool solid, bool outside, bool explodes)
        {
            Flammability = flammability;
            Material = material;
            State = state;
            _solid = solid;
            Outside = outside;
            EXPLODES = explodes;
        }

        public Tile(Layer1TileType layer1, Layer2TileType layer2, Layer3TileType layer3)
        {
            Flammability = 0;
            Material = 0;
            State = FireState.Unburned;

            SetLayer1(layer1);
            SetLayer2(layer2);
            SetLayer3(layer3);
        }

        private void SetLayer1(Layer1TileType type)
        {
            switch (type)
            {
                case Layer1TileType.Carpet1:
                    _texture1 = "Carpet 1";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    SetFlammability(.004f);
                    SetSolidity(false);
                    break;
                case Layer1TileType.Carpet2:
                    _texture1 = "Carpet 2";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    SetFlammability(.004f);
                    SetSolidity(false);
                    break;
                case Layer1TileType.Tile1:
                    _texture1 = "Tiled Floor 1";
                    SetMaterial(Global.rand.Next(1000, 2000));
                    SetFlammability(.0001f);
                    SetSolidity(false);
                    break;
                case Layer1TileType.Tile2:
                    _texture1 = "Tiled Floor 2";
                    SetMaterial(Global.rand.Next(1000, 2000));
                    SetFlammability(.0001f);
                    SetSolidity(false);
                    break;
                case Layer1TileType.WoodFloor:
                    _texture1 = "Wooden Floor";
                    SetMaterial(Global.rand.Next(3000, 4000));
                    SetFlammability(.003f);
                    SetSolidity(false);
                    break;
                case Layer1TileType.GraniteWall:
                    _texture1 = "Granite Wall";
                    SetMaterial(0);
                    SetFlammability(0);
                    SetSolidity(true);
                    break;
                case Layer1TileType.WoodWall:
                    _texture1 = "Wooden Wall";
                    SetMaterial(Global.rand.Next(5000, 6000));
                    SetFlammability(.0001f);
                    SetSolidity(true);
                    break;
                case Layer1TileType.Outside:
                    _texture1 = "Empty";
                    SetMaterial(0);
                    SetFlammability(0);
                    SetSolidity(false);
                    Outside = true;
                    break;
            }
        }

        private void SetLayer2(Layer2TileType type)
        {
            switch (type)
            {
                case Layer2TileType.Desk:
                    _texture2 = "Desk";
                    SetMaterial(Global.rand.Next(5000, 6000));
                    SetFlammability(.003f);
                    SetSolidity(false);
                    break;
                case Layer2TileType.Sofa:
                    _texture2 = "Couch";
                    SetMaterial(Global.rand.Next(4000, 5000));
                    SetFlammability(.1f);
                    SetSolidity(false);
                    break;
                case Layer2TileType.Plant:
                    _texture2 = "Plant";
                    SetMaterial(Global.rand.Next(1000, 2000));
                    SetFlammability(.0001f);
                    SetSolidity(false);
                    break;
                case Layer2TileType.OpenHorizontalDoor:
                    _texture2 = "Open Horizontal Door";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    SetFlammability(.0001f);
                    SetSolidity(false);
                    break;
                case Layer2TileType.OpenVerticalDoor:
                    _texture2 = "Open Vertical Door";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    SetFlammability(.0001f);
                    SetSolidity(false);
                    break;
                case Layer2TileType.ClosedHorizontalDoor:
                    _texture2 = "Closed Horizontal Door";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    Flammability = .0001f;
                    SetSolidity(true);
                    break;
                case Layer2TileType.ClosedVerticalDoor:
                    _texture2 = "Closed Vertical Door";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    Flammability = .0001f;
                    SetSolidity(true);
                    break;
                case Layer2TileType.TrashCan:
                    _texture2 = "Trash Can";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    SetFlammability(.2f);
                    SetSolidity(false);
                    break;
                case Layer2TileType.Newspaper:
                    _texture2 = "Newspaper";
                    SetMaterial(Global.rand.Next(1000, 2000));
                    SetFlammability(.2f);
                    SetSolidity(false);
                    break;
                case Layer2TileType.Flammables:
                    _texture2 = "Flammables";
                    Material = 100;   // Gas cans need to blow up fast, so override the material
                    SetFlammability(1);
                    SetSolidity(false);
                    EXPLODES = true;
                    break;
                case Layer2TileType.Empty:
                    _texture2 = "Empty";
                    SetMaterial(0);
                    SetFlammability(0);
                    SetSolidity(false);
                    break;
            }
        }

        private void SetLayer3(Layer3TileType type)
        {
            switch (type)
            {
                case Layer3TileType.Computer:
                    _texture3 = "Computer";
                    SetMaterial(Global.rand.Next(2000, 3000));
                    SetFlammability(.002f);
                    SetSolidity(false);
                    break;
                case Layer3TileType.Blotter:
                    _texture3 = "Blotter";
                    SetMaterial(Global.rand.Next(1000, 2000));
                    SetFlammability(.2f);
                    SetSolidity(false);
                    break;
                case Layer3TileType.Plant:
                    _texture3 = "Desk Plant";
                    SetMaterial(Global.rand.Next(500, 1000));
                    SetFlammability(.0001f);
                    SetSolidity(false);
                    break;
                case Layer3TileType.Laptop:
                    _texture3 = "Laptop";
                    SetMaterial(Global.rand.Next(1000, 2000));
                    SetFlammability(.002f);
                    SetSolidity(false);
                    break;
                case Layer3TileType.Empty:
                    _texture3 = "Empty";
                    SetMaterial(0);
                    SetFlammability(0);
                    SetSolidity(false);
                    break;
            }
        }

        private void SetMaterial(int material)
        {
            Material += material;
        }

        private void SetFlammability(float flammability)
        {
            if (flammability > Flammability)
                Flammability = flammability;
        }

        private void SetSolidity(bool solid)
        {
            _solid |= solid;
        }

        private string TextureEnding()
        {
            return State == FireState.Burnt ? "b" : String.Empty;
        }

        public void UpdateBurning(int material, FireState state)
        {
            Material = material;
            State = state;
        }

        /// <summary>
        /// Fights fire and returns whether there was fire to fight
        /// </summary>
        /// <returns></returns>
        public bool HitByWater()
        {
            if (State == FireState.Burning && Global.rand.Next(0,1001) >= 1000)
            {
                State = FireState.Unburned;
                return true;
            }
            Flammability /= 1.001f;
            return false;

        }

        public bool ReadyToExplode
        {
            get { return EXPLODES && Material <= 0; }          
        }
    }
}
