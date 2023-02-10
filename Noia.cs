using System;
using System.Drawing;
using System.Collections.Generic;

public class Noia : Player
{
    public Noia(PointF location) :
        base(location, Color.Green, Color.LightSeaGreen, "Maluco")
    { }

    int i = 0;

    const float screenWidth = 1280;
    const float screenHeigth = 800;

    bool isloading = false;
    bool findWay = false;
    bool turretMode = false;
    bool inCorner = false;
    bool moving = false;
    
    PointF? ultimoDano = null;

    PointF topLeft = new PointF(0, 0);
    PointF topRight = new PointF(screenWidth, 0);
    PointF botLeft = new PointF(0, screenHeigth);
    PointF botRight = new PointF(screenWidth, screenHeigth);

    int bulletCounter = 0;
    int maxBullets = 50;

    int offset = 10;
    int restCount = 0;


    PointF pontoSelecionado;


    protected override void loop()
    {
        List<PointF> pontos = new List<PointF> { topLeft, topRight, botRight, botLeft };


        if (Energy < 50)
        {
            StopTurbo();
            bulletCounter = maxBullets;
            turretMode = false;
            isloading = true;
        } else {
            
        }

        if (isloading)
        {
            if (Energy > 40)
            {
                isloading = false;
            }
            else return;
        }

        if(LastDamage != ultimoDano)
        {   
            bulletCounter = maxBullets;
            StartTurbo();
            pontos.Reverse();       
            inCorner = true;
            turretMode = false;     
            ultimoDano = LastDamage;
        }

        if ((Location.X >= pontoSelecionado.X - 10 && Location.X <= pontoSelecionado.X + 10)
            && (Location.Y >= pontoSelecionado.Y - 10 && Location.Y <= pontoSelecionado.Y + 10))
        {
            // if(shootCounter == 50)
            //     shootCounter = 0;
            if(bulletCounter == maxBullets)
            {
                inCorner = true;
                turretMode = false;
            }
            else
                turretMode = true;
            StopMove();
        }

        if (!findWay)
        {
            turretMode = false;
            if (Location.X < screenWidth / 2 && Location.Y < screenHeigth / 2)
            {
                StartMove(topLeft);
                pontoSelecionado = topLeft;
            }

            else if (Location.X >= screenWidth / 2 && Location.Y < screenHeigth / 2)
            {
                StartMove(topRight);
                pontoSelecionado = topRight;
            }

            else if (Location.X >= screenWidth / 2 && Location.Y >= screenHeigth / 2)
            {
                StartMove(botRight);
                pontoSelecionado = botRight;
            }

            else if (Location.X < screenWidth / 2 && Location.Y >= screenHeigth / 2)
            {
                StartMove(botLeft);
                pontoSelecionado = botLeft;
            }

            findWay = true;
        }

        if(inCorner && !turretMode)
        {
            var index = pontos.IndexOf(pontoSelecionado) + 1;
            if(index >= pontos.Count)
                index = 0;
            
            pontoSelecionado = pontos[index];
            StartMove(pontoSelecionado);
            
            inCorner = false;
            bulletCounter = 0;
        }

        if (bulletCounter < maxBullets && turretMode)
        {
            SizeF direction = new SizeF(
                (float)Math.Cos((5f * i++) * (2 * Math.PI) / 360),
                (float)Math.Sin((5f * i++) * (2 * Math.PI) / 360)
            );

            bulletCounter++;

            if(pontoSelecionado == botRight)
            {

                if(direction.Height > 0)
                {
                    direction.Height *= -1;
                } 

                if(direction.Width > 0)
                {
                    direction.Width *= -1;
                }

                Shoot(Location + direction);
            } 
            else if(pontoSelecionado == topLeft)
            {
                if(direction.Height < 0)
                    direction.Height *= -1;

                if(direction.Width < 0)
                    direction.Width *= -1;

                Shoot(Location + direction);
            }
            else if(pontoSelecionado == topRight)
            {
                if(direction.Height < 0)
                    direction.Height *= -1;
            
                if(direction.Width > 0)
                    direction.Width *= -1;

                Shoot(Location + direction);
            }
            else if(pontoSelecionado == botLeft)
            {
                if(direction.Height > 0)
                    direction.Height *= -1;
            
                if(direction.Width < 0)
                    direction.Width *= -1;

                Shoot(Location + direction);
            }
        }
    }
}
