using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{

    using GXPEngine;
    using System;

    public class Button : Sprite
    {
        public event Action onClick;

        private bool isMouseDown = false;

        public Button(string text, int width, int height) : base("button.png")
        {
            SetScaleXY(width / (float)width, height / (float)height);
            SetOrigin(width / 2, height / 2);

            
            EasyDraw label = new EasyDraw(width, height);
            label.TextAlign(CenterMode.Center, CenterMode.Center);
            label.Text(text, width / 2, height / 2);
            AddChild(label);
        }

        void Update()
        {
           
            if (HitTestPoint(Input.mouseX, Input.mouseY))
            {
                if (Input.GetMouseButtonDown(0) && !isMouseDown)
                {
                   
                    onClick?.Invoke();
                    isMouseDown = true;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isMouseDown = false;
                }
            }
        }
    }
    public class LevelSelectionScreen : GameObject
    {

        public LevelSelectionScreen()
        {
            CreateLevelButtons();
        }


        private void CreateLevelButtons()
        {
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 20;

            
            for (int i = 0; i < 4; i++)
            {
                Button levelButton = new Button("Level " + (i + 1), buttonWidth, buttonHeight);
                levelButton.SetXY((game.width - buttonWidth) / 2, (buttonHeight + buttonSpacing) * i + (game.height - (buttonHeight + buttonSpacing) * 4) / 2);
                levelButton.onClick += () => LoadGameLevel(i + 1);
                AddChild(levelButton);
            }
        }

        private void LoadGameLevel(int levelIndex)
        {
            
            GameLevel gameLevel = new GameLevel();

            
            game.AddChild(gameLevel);

            
            LateDestroy();
        }
    }
}
