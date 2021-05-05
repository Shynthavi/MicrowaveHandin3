using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrationtest
{
    public class IntegrationStep1
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private UserInterface _userInterface;


        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _cookController = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();

            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);
        }

        //1A
        #region Door
        //Door
        [Test]
        public void Door_Opened_Light_On()
        {
            //Act
            _door.Open();
            //Assert
            _light.Received(1).TurnOn();
        }


        [Test]
        public void Door_Closed_LightOff()
        {
            //Act
            _door.Open();
            _door.Close();
            //Assert
            _light.Received(1).TurnOff();
        }


        [Test]
        public void Door_Opened_Cooking_Started()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();

            //Assert
            Assert.Multiple(() =>
            {
                _display.Received(1).Clear();
                _cookController.Received(1).Stop();
            });
        }

        #endregion

        //1B
        #region PowerButton

        [Test]
        public void PowerButton_On_ShowPower()
        {
            //Act
            _powerButton.Press();

            //Assert
            _display.Received(1).ShowPower(50);
        }




        #endregion


    }

}