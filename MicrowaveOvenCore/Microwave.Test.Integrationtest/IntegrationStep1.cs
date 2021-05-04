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
        private Button _powerButton;
        private Button _timeButton;
        private Button _cancelStartButton;
        private Door _door;
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private UserInterface _userInterface;
    }

    [SetUp]
    public void Setup()
    {
        _door= new Door();
        _powerButton = new Button();
        _timeButton = new Button();
        _startCancelButton = new Button();
        _display = Substitute.For<IDisplay>();
        _light = Substitute.For<ILight>();
        _cookController = Substitute.For<ICookController>();
        sut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
    }
}