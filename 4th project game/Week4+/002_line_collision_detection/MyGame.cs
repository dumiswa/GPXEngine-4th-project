using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

public class MyGame : Game
{	

	static void Main() {
		new MyGame().Start();
	}

	EasyDraw _text;

	List<Sprite> panels;
	public List<Sprite> rocks;
	Sprite menuBackground;
	Sprite[] levelBackgrounds = new Sprite[4];
	Sprite[] levelSelection = new Sprite[10];
	Sprite satellite;
	public Sprite smallAsteroid;

	AnimationSprite explosion;
	public bool toExplode = false;

	public List<LineSegment> pannels;
	public List<LaserPath> laserPaths;

	int currentScene = 0;

	public String textToShow = "no text";

	Vec2 start;

	bool isDragging;
	public int whichPanel;

	public MyGame () : base(1000, 750, false,false)
	{

		menuBackground = new Sprite("../../../assets/menuBackground.png");

		levelBackgrounds[0] = new Sprite("../../../assets/bg-level1.png");
		levelBackgrounds[1] = new Sprite("../../../assets/bg-level2.png");
		levelBackgrounds[2] = new Sprite("../../../assets/bg-level3.png");
		levelBackgrounds[3] = new Sprite("../../../assets/bg-level4.png");

		levelSelection[0] = new Sprite("../../../assets/3_locked_1st_selected.png");
		levelSelection[1] = new Sprite("../../../assets/2_locked_1st_selected.png");
		levelSelection[2] = new Sprite("../../../assets/2_locked_2nd_selected.png");
		levelSelection[3] = new Sprite("../../../assets/1_locked_1st_selected.png");
		levelSelection[4] = new Sprite("../../../assets/1_locked_2nd_selected.png");
		levelSelection[5] = new Sprite("../../../assets/1_locked_3rd_selected.png");
		levelSelection[6] = new Sprite("../../../assets/0_locked_1st_selected.png");
		levelSelection[7] = new Sprite("../../../assets/0_locked_2nd_selected.png");
		levelSelection[8] = new Sprite("../../../assets/0_locked_3rd_selected.png");
		levelSelection[9] = new Sprite("../../../assets/0_locked_4th_selected.png");

		satellite = new Sprite("../../../assets/satellite.png");

		smallAsteroid = new Sprite("../../../assets/small_asteroid.png");

		rocks = new List<Sprite>();
		panels = new List<Sprite>();
		pannels = new List<LineSegment>();

		LoadScene(0, -1);

		//start = new Vec2(game.width / 2, game.height - 20);
		//start = new Vec2(0, 0);

		//aim = new LineSegment(start, end, 0xff00ff00, 2);
		//AddChild(aim);

		laserPaths = new List<LaserPath>();

		//_text = new EasyDraw(250, 250);
  //      _text.TextAlign(CenterMode.Min, CenterMode.Min);
  //      AddChild(_text);

    
    }


	public void AddRocks(Vec2 pPos) {

	}

	public void AddLaserPath(Vec2 pStart, Vec2 pDirection)
	{
		LaserPath laser = new LaserPath(pStart, pStart + pDirection, pDirection);
		AddChild(laser);
		laserPaths.Add(laser);
	}

	void LoadScene(int sceneNumber, int lastScene)
	{

		switch (sceneNumber)
		{
			//menu
			case 0:

				menuBackground.SetXY(0, 0);
				AddChild(menuBackground);

				currentScene = 0;

			break;

			//level selection
			case 1:

				if (lastScene == 0)
				{
					AddChild(levelBackgrounds[0]);

					levelSelection[0].SetXY(115, 200);
					AddChild(levelSelection[0]);
					menuBackground.Remove();
				}

				if (lastScene == 2)
				{

					foreach (Sprite sprite in rocks)
					{
						sprite.Destroy();
					}
					rocks.Clear();

					foreach (Sprite sprite1 in panels)
					{
						sprite1.Destroy();
					}
					panels.Clear();

					foreach (LineSegment lineSegment in pannels)
					{
						lineSegment.Remove();
					}
					pannels.Clear();

					satellite.Remove();
					explosion.Destroy();

					toExplode = false;

					AddChild(levelBackgrounds[0]);

					levelSelection[2].SetXY(115, 200);
					AddChild(levelSelection[2]);
					menuBackground.Remove();
				}

				else
				{

				}

				currentScene = 1;

				break;

			//1st level
			case 2:
				

                Sprite[] sprites = FindObjectsOfType<Sprite>();
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].Remove();
                }

				AddChild(levelBackgrounds[0]);

				for (int i = 0; i < 8; i++) {
					Sprite sprite = new Sprite("../../../assets/rock.png");
					sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
					sprite.SetScaleXY(0.8f, 0.8f);
					AddChild(sprite);
					rocks.Add(sprite);
				}
				rocks[0].SetXY(600, 70);
				rocks[1].SetXY(500, 128);
				rocks[2].SetXY(219, 213);
				rocks[3].SetXY(409, 277);
				rocks[4].SetXY(690, 300);
				rocks[5].SetXY(840, 392);
				rocks[6].SetXY(271, 436);
				rocks[7].SetXY(476, 494);

				for (int i = 0; i < 2; i++) {

					LineSegment lineSegment = new LineSegment(new Vec2(0, 0), new Vec2(0, 0), 0xffffffff);
					AddChild(lineSegment);
					pannels.Add(lineSegment);

					Sprite sprite = new Sprite("../../../assets/panel.png");
					sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
					sprite.SetScaleXY(0.5f, 0.5f);
					AddChild(sprite);
					panels.Add(sprite);
				}
				panels[0].SetXY(100, 700);
				panels[1].SetXY(800, 700);

				satellite.SetOrigin(0, satellite.height / 2);
				satellite.SetXY(game.width / 2, game.height - 20);
				satellite.SetScaleXY(0.5f, 0.5f);
				AddChild(satellite);

				smallAsteroid.SetScaleXY(0.8f, 0.8f);
				smallAsteroid.SetOrigin(smallAsteroid.width / 2, smallAsteroid.height / 2);
				smallAsteroid.SetXY(780, 30);
				AddChild(smallAsteroid);

				explosion = new AnimationSprite("../../../assets/Explosion.png", 6, 1);
				explosion.SetOrigin(explosion.width / 2, explosion.height / 2);
				explosion.SetScaleXY(0.8f, 0.8f);
				explosion.SetXY(780, 30);
				AddChild(explosion);


				currentScene = 2;

				break;

		}


	}



	void Update()
	{
		Vec2 aimDirection = new Vec2(Mathf.Cos(satellite.rotation * (Mathf.PI / 180)), Mathf.Sin(satellite.rotation * (Mathf.PI / 180)));
		start = new Vec2(game.width / 2, game.height - 20) + aimDirection * 100;

		if (currentScene == 0)
        {
            if (Input.GetMouseButtonDown(0) && new Vec2(Input.mouseX - 312, Input.mouseY - 526).Length() <= 60)
            {
                LoadScene(1, currentScene);
            }
        }



        if (currentScene == 1)
        {

            if (Input.GetMouseButtonDown(0) && new Vec2(Input.mouseX - 189, Input.mouseY - 275).Length() <= 70)
            {
                LoadScene(2, currentScene);
            }
        }



        if (currentScene == 2) {

			if (toExplode)
			{
				explosion.SetCycle(0, 5);
				explosion.Animate(0.1f);

                if (explosion.currentFrame == 4)
                {
					smallAsteroid.Remove();
					LoadScene(1, 2);
                }
            }
			else {
				explosion.SetFrame(5);
			}
			

			if (satellite != null && laserPaths.Count == 0) {
				satellite.rotation = Mathf.Atan2(Input.mouseY - satellite.y, Input.mouseX - satellite.x) * (180 / Mathf.PI);
			}

			if (Input.GetMouseButtonDown(0))
			{
				for (int i = 0; i < 2; i++) {

					if (Input.mouseX >= panels[i].x - panels[i].width / 2 && Input.mouseX <= panels[i].x + panels[i].width / 2 && Input.mouseY >= panels[i].y - panels[i].height / 2 && Input.mouseY <= panels[i].y + panels[i].height / 2) {
						isDragging = true;
						whichPanel = i;
					}
				}
			}

			if (isDragging && laserPaths.Count == 0) {
				panels[whichPanel].SetXY(Input.mouseX, Input.mouseY);

                if (Input.GetKey(Key.A))
                {
					panels[whichPanel].rotation = panels[whichPanel].rotation - 2;

				}
                if (Input.GetKey(Key.D))
                {
					panels[whichPanel].rotation = panels[whichPanel].rotation + 2;
				}

				if (panels[whichPanel].rotation > 360) { panels[whichPanel].rotation = 0; }
				if (panels[whichPanel].rotation < 0) { panels[whichPanel].rotation = 360; }

				Vec2 center = new Vec2(panels[whichPanel].x, panels[whichPanel].y);
				Vec2 direction = new Vec2(Mathf.Cos(panels[whichPanel].rotation * (Mathf.PI / 180)), Mathf.Sin(panels[whichPanel].rotation * (Mathf.PI / 180)));
				pannels[whichPanel].start = center + direction * 50;
				pannels[whichPanel].end = center + direction * -50;

			}


			if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;

				for (int i = 0; i < 8; i++) {

					if (new Vec2(Input.mouseX - rocks[i].x, Input.mouseY - rocks[i].y).Length() <= 70) {

						pannels[whichPanel].start = new Vec2(0, 0);
						pannels[whichPanel].end = new Vec2(0, 0);

						if (whichPanel == 0) {
							panels[whichPanel].SetXY(100, 700);
						}
						if (whichPanel == 1)
						{
							panels[whichPanel].SetXY(800, 700);
						}
					}
				}

			}




        }

        if (Input.GetKeyDown(Key.W))
        {
			if (laserPaths.Count > 0)
			{
				DestroyLaserPath();
			}

			Vec2 mousePosition = new Vec2(Input.mouseX, Input.mouseY);
            AddLaserPath(start, (mousePosition - start).Normalized());
        }


        textToShow = laserPaths.Count.ToString();

		//_text.Clear(Color.Transparent);
		//_text.Text(textToShow, 0, 0);
	}


	public void DestroyLaserPath() {

		foreach (LaserPath laser in laserPaths)
		{
			laser.Destroy();
		}
		laserPaths.Clear();
	}
	


}

