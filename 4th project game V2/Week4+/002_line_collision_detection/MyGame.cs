using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;

public class GameLevel : GameObject
{
    EasyDraw _text;

    public LineSegment[] pannels = new LineSegment[4];
    public List<LaserPath> laserPaths;
    LineSegment aim;

    public String textToShow = "no text";

    float angle = 0;
    Vec2 pivot;

    Vec2 start;
    Vec2 end;

    Vec2 pivotDifference;
    Vec2 startDifference;
    Vec2 endDifference;
    Vec2 mousePositionPressed;
    LineSegment pannel = null;
    

    public class MyGame : Game
    {
        private Sprite menuBackground;
        private ButtonLevelChange _levelButton;

        public MyGame() : base(1200, 900, false)
        {
            
            _levelButton = new ButtonLevelChange("button.png", 1);
            AddChild(_levelButton);

            menuBackground = new Sprite("Home_Menu.png");
            AddChild(menuBackground);

            _levelButton.OnButtonClick += PlayButtonClicked;
        }

        private void PlayButtonClicked()
        {           
            menuBackground.LateRemove();
          
        }

    }
    public GameLevel() 
    {     
        pannels[0] = new LineSegment(new Vec2(1000, 300), new Vec2(800, 300), 0xffffffff);
        AddChild(pannels[0]);

        pannels[1] = new LineSegment(new Vec2(800, 100), new Vec2(400, 200), 0xffffffff);
        AddChild(pannels[1]);

        pannels[2] = new LineSegment(new Vec2(500, 600), new Vec2(300, 500), 0xffffffff);
        AddChild(pannels[2]);

        pannels[3] = new LineSegment(new Vec2(600, 600), new Vec2(700, 550), 0xffffffff);
        AddChild(pannels[3]);

        start = new Vec2(game.width / 2, 800);
        end = new Vec2(game.width / 2, 800);

        aim = new LineSegment(start, end, 0xff00ff00, 2);
        AddChild(aim);

        laserPaths = new List<LaserPath>();

        _text = new EasyDraw(250, 250);
        _text.TextAlign(CenterMode.Min, CenterMode.Min);
        AddChild(_text);

    }

  

    public void AddLaserPath(Vec2 pStart, Vec2 pDirection)
    {
        LaserPath laser = new LaserPath(pStart, pStart + pDirection, pDirection, this);
        AddChild(laser);
        laserPaths.Add(laser);
    }

    void Update()
    {

        if (Input.GetKeyDown(Key.W))
        {
            Vec2 mousePosition = new Vec2(Input.mouseX, Input.mouseY);
            AddLaserPath(start, (mousePosition - start).Normalized());
        }

        if (Input.GetKeyUp(Key.W))
        {

            textToShow = "no text";

            foreach (LaserPath laser in laserPaths)
            {
                laser.Destroy();
            }
            laserPaths.Clear();
        }

        if (Input.GetMouseButtonDown(0))
        {

            mousePositionPressed = new Vec2(Input.mouseX, Input.mouseY);
            float distanceToCheck = float.MaxValue;

            for (int i = 0; i < pannels.Length; i++)
            {
                if ((mousePositionPressed - ((pannels[i].end - pannels[i].start) / 2 + pannels[i].start)).Length() < 100)
                {
                    if ((mousePositionPressed - ((pannels[i].end - pannels[i].start) / 2 + pannels[i].start)).Length() < distanceToCheck)
                    {
                        distanceToCheck = (mousePositionPressed - ((pannels[i].end - pannels[i].start) / 2 + pannels[i].start)).Length();
                        pannel = pannels[i];
                    }
                }
            }

            if (pannel != null)
            {
                pannel.color = 0xff00ff00;
                startDifference = mousePositionPressed - pannel.start;
                endDifference = mousePositionPressed - pannel.end;
                
            }
        }

        if (Input.GetMouseButton(0) && pannel != null)
        {


            pannel.start = new Vec2(Input.mouseX, Input.mouseY) - startDifference;
            pannel.end = new Vec2(Input.mouseX, Input.mouseY) - endDifference;

            

            if (Input.GetKey(Key.A))
            {

                angle = -1f;
                Vec2 rotatedEnd = pannel.end.RotateAroundPoint(pivot, angle);
                pannel.end = rotatedEnd;

                Vec2 rotatedStart = pannel.start.RotateAroundPoint(pivot, angle);
                pannel.start = rotatedStart;
            }
            if (Input.GetKey(Key.D))
            {
                angle = 1f;
                Vec2 rotatedEnd = pannel.end.RotateAroundPoint(pivot, angle);
                pannel.end = rotatedEnd;

                Vec2 rotatedStart = pannel.start.RotateAroundPoint(pivot, angle);
                pannel.start = rotatedStart;
            }
        }

        if (Input.GetMouseButtonUp(0) && pannel != null)
        {
            pannel.color = 0xffffffff;
            pannel = null;
        }



        textToShow = laserPaths.Count.ToString();

        _text.Clear(Color.Transparent);
        _text.Text(textToShow, 0, 0);
    }

    static void Main()
    {
        new MyGame().Start();
    }
}







public class ButtonLevelChange : Sprite
{
    private int _nextLevelIndex;

    public event Action OnButtonClick;

    public ButtonLevelChange(string filename, int nextLevelIndex) : base(filename)
    {
        _nextLevelIndex = nextLevelIndex;
        SetXY(game.width / 2 - 50, game.height / 2);
    }

    private void ButtonClickHandler()
    {
        OnButtonClick?.Invoke();
        LevelSelectionScreen levelSelectionScreen = new LevelSelectionScreen();
        game.AddChild(levelSelectionScreen);
        LateDestroy();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float mouseX = Input.mouseX;
            float mouseY = Input.mouseY;

            if (HitTestPoint(mouseX, mouseY))
            {
                ButtonClickHandler();
            }
        }
    }
}




