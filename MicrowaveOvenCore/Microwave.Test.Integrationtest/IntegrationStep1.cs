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
        public void PowerButton_PressOnce_ShowPower()
        {
            //Act
            _powerButton.Press();

            //Assert
            _display.Received(1).ShowPower(50);
        }

        [Test]
        public void PowerButton_PressThrice_ShowPower()
        {
            //Act
            _powerButton.Press();
            _powerButton.Press();

            //Assert
            _display.Received(1).ShowPower(150);
        }

        [Test]
        public void PowerButton_PowerLevel700_ShowPower()
        {
            //Act
            for (int i = 0; i < 14; i++)
            {
                _powerButton.Press();
            }

            //Assert
            _display.Received(1).ShowPower(50);
        }

        #endregion

        //1C
        #region TimeButton

        [Test]
        public void TimerButton_PressOnce_ShowTime()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();

            //Assert
            _display.Received(1).ShowTime(1,0);
        }

        [Test]
        public void TimerButton_PressTwice_ShowTime()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();

            //Assert
            _display.Received(1).ShowTime(2, 0);
        }

        [Test]
        public void TimerButton_Press_PowerNotPressed()
        {
            //Act
            _timeButton.Press();

            //Assert
            _display.DidNotReceive().ShowTime(1, 0);
        }

        #endregion

        //1D
        #region StartCancelButton

        [Test]
        public void StartCancelButton_Press_TurnOn_StartCooking()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            //Assert
            Assert.Multiple(() =>
            {
                _light.TurnOn();
                _cookController.Received(1).StartCooking(50,1*60);
            });
        }

        [Test]
        public void StartCancelButton_TimerPowerPressedTwice_StartCooking()
        {
            //Act
            _powerButton.Press();
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            //Assert
            Assert.Multiple(() =>
            {
                _light.TurnOn();
                _cookController.Received(1).StartCooking(100, 2 * 60);
            });
        }

        [Test]
        public void StartCancelButton_Press_CookingStarted()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();

            //Assert
            Assert.Multiple(() =>
            {
                _light.TurnOff();
                _cookController.Received(1).Stop();
                _display.Clear();
            });
        }

        #endregion
    }

}