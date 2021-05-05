using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
namespace Microwave.Test.Integrationtest
{
    [TestFixture]
    public class IntegrationStep3
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
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();

            _cookController = new CookController(_timer, _display, _powerTube);

            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);

            //Property dependency injection
            _cookController.UI = _userInterface;
        }


        [Test]
        public void CookController_StartCooking()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            //Assert
            Assert.Multiple(() =>
            {
                _timer.Received(1).Start(1*60*1000/60);
                _powerTube.Received(1).TurnOn(50);
            });
        }

        [Test]
        public void CookController_OpenDoor_Stop()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();

            //Assert
            Assert.Multiple(() =>
            {
                _timer.Received(1).Stop();
                _powerTube.Received(1).TurnOff();
            });
        }


        [Test]
        public void CookController_StartCancelPress_Stop()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();

            //Assert
            Assert.Multiple(() =>
            {
                _timer.Received(1).Stop();
                _powerTube.Received(1).TurnOff();
            });
        }

    }
}