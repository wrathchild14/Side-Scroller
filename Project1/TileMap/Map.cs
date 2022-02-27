﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.TileMap;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.TileMap
{
    class Map
    {
        private List<CollisionTiles> collison_tiles_ = new List<CollisionTiles>();

        public List<CollisionTiles> CollisionTiles
        {
            get { return collison_tiles_; }
        }

        private int width_, height_;

        public int Width
        {
            get { return width_; }
        }

        public int Height
        {
            get { return height_; }
        }

        public Map()
        {
        }

        public void Generate(int[,] map, int size)
        {
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0)
                        collison_tiles_.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size)));

                    width_ = (x + 1) * size;
                    height_ = (y + 1) * size;
                }
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            foreach (CollisionTiles tile in collison_tiles_)
                tile.Draw(sprite_batch);
        }
    }
}