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

        #region SetUp
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
        #endregion

        #region PowerButton Pressed
        //Denne test fandt fejlen "Input out of range". Da de 3. tryk på powerButton overskrider max på 100. jf. det oprindelige program.
        //Dette er blevet ændret, så range nu godkender værdier fra 50 til 700 jf. øvelsesvejledningen.

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(14)]
        [TestCase(15)]
        [Test]
        public void StartCooking_PowerTube_On(int ButtonPress)
        {
            int power;
            //Arrange
            if (ButtonPress > 14)
            {
                power = 50;
            }
            else
            {
                power = ButtonPress * 50;
            }
            
            _door.Close();

            //Act
            for(int i =0; i<ButtonPress;i++)
            {
                _powerButton.Press();
            }

            _timeButton.Press();
            _startCancelButton.Press();
           
            //Assert
            Assert.Multiple(() =>
            {
                _output.Received(1).OutputLine("Light is turned on");
                _output.Received(1).OutputLine($"PowerTube works with {power}");
            });

        }
        #endregion //

        #region 1. Extension Door opened while cooking

        //Denne test er til første extension jf. sekvensdiagrammet - hvor brugeren åbner door mens cooking er igang

        [Test]
        public void CookingStarted_DoorOpen_PowerTube_Off()
        {
            //Arrange
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            //Act
            _door.Open();

            //Assert
            _output.Received(1).OutputLine($"PowerTube turned off");
        }
        #endregion

        #region 2. Extension cancelButton pressed while cooking
        //2. Udvidelse jf. sekvensdiagrammet i øvelsesvejledningen. Brugeren trykker cancel, mens cooking er igang

        [Test]
        public void CookingStarted_StartCancelButton_IsPressed()
        {
            //Arrange
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            //Act
            _startCancelButton.Press();

            //Assert
            _output.Received(1).OutputLine($"PowerTube turned off");
        }
        #endregion
    }
}
