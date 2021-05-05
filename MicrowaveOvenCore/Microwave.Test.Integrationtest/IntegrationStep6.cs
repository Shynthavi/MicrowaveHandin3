using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrationtest
{
    [TestFixture]
    public class IntegrationStep6
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
        private StringWriter _sw;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _output = new Output();
            _display = new Display(_output);
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _timer = new Microwave.Classes.Boundary.Timer();

            _cookController = new CookController(_timer, _display, _powerTube);

            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);

            //Property dependency injection
            _cookController.UI = _userInterface;

            _sw = new StringWriter();
            Console.SetOut(_sw);
        }


        [Test]
        public void Display_Light_PowerTube_Output()
        {
            //Arrange
            var expected = "Display shows: 50 W\r\nDisplay shows: 01:00\r\nLight is turned on" +
                           "\r\nPowerTube works with 50\r\nDisplay shows: 00:00\r\nPowerTube turned off" +
                           "\r\nDisplay cleared\r\nLight is turned off\r\n";
            var output = new StringWriter();
            Console.SetOut(output);

            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            Thread.Sleep(1200);

            //Assert
            Assert.That(output.ToString(), Is.EqualTo(expected));
        }


    }
}