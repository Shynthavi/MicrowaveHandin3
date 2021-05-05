using System;
using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrationtest
{
    [TestFixture]
    public class IntegrationStep5
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
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _timer = new Microwave.Classes.Boundary.Timer();

            _cookController = new CookController(_timer, _display, _powerTube);

            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);

            //Property dependency injection
            _cookController.UI = _userInterface;
        }


        [Test]
        public void OnTimerTick_CookController()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            Thread.Sleep(3000);

            //Assert
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("00:00")));

        }

        [Test]
        public void OnTimerTick_CookController_3()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            Thread.Sleep(3000);

            //Assert
            _output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("02:00"))); //Receives two, once when timer increments, and second time when timer decrements

        }
    }
}