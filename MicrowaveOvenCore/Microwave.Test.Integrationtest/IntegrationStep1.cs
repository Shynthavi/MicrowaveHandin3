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
        private IButton _cancelStartButton;
        private IDoor _door;
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private UserInterface _userInterface;


        [SetUp]
        public void Setup()
        {
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _cancelStartButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _cookController = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            
            _userInterface = new UserInterface(_powerButton, _timeButton, _cancelStartButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void Door_Opened_Light_On()
        {
            _door.Open();
            _light.TurnOn();
        }


        [Test]
        public void Door_Closed_LightOff()
        {
            _door.Open();
            //Act
            _door.Close();
            //Assert
            _light.TurnOff();
        }

        [Test]
        public void PowerButton_IsPressed_ShowPower()
        {
            //Act
            _powerButton.Press();
            //Assert
            _display.ShowPower(5);
        }

        [Test]
        public void TimeButton_IsPressed_TimerOn()
        {
            //Arrange
            _powerButton.Press();
            //Act
            _timeButton.Press();
            //Assert
            _display.ShowTime(5,30);
        }


        [Test]
        public void CancelStartButton_IsPressed_LightOn_CookingStart()
        {
            //Arrange
            _powerButton.Press();
            _timeButton.Press();
            //Act
            _cancelStartButton.Press();
            //Assert
            _light.TurnOn();
            _cookController.StartCooking(5, 5);
        }


    }

   
}