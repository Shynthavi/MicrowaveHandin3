using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrationtest
{
    public class IntegrationStep4
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private CookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private IUserInterface _userInterface;
        private IOutput _output;
        private IPowerTube _powerTube;
        private ITimer _timer;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _timer = Substitute.For<ITimer>();
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;

        }

        [Test]
        public void StartCooking_PowerTube_On()
        {
            //Arrange
            int power = 50;
            _door.Close();
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            //Assert
            //_output.Received(1).OutputLine($"PowerTube works with {power}");

            Assert.Multiple(() =>
            {
                _output.Received(1).OutputLine("Light is turned on");
                _output.Received(1).OutputLine($"PowerTube works with {power}");
            });

        }

        //Denne test finder fejlen "Input out of range". Da de 3 tryk på powerButton overskrider max på 100. 
        [Test]
        public void StartCooking_PowerTube_On_PowerPressedX3()
        {
            //Arrange
            int power = 150;
            _door.Close();
            //Act
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            //Assert
            //_output.Received(1).OutputLine($"PowerTube works with {power}");

            Assert.Multiple(() =>
            {
                _output.Received(1).OutputLine("Light is turned on");
                _output.Received(1).OutputLine($"PowerTube works with {power}");
            });

        }


        [Test]
        public void CookingStarted_DoorOpen_PowerTube_Off()
        {
            //Arrange
            _door.Close();

            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();

            //Assert
            _output.Received(1).OutputLine($"PowerTube turned off");
        }

   
    }
}
