using Godot;
using System;

public partial class GameUI : CanvasLayer
{
    private Control _startMenu;
    private Control _gameOverMenu;
    private Control _pauseMenu;
    private Label _scoreLabel;
    private Label _highScoreLabel;
    public override void _Ready()
    {

        ProcessMode = ProcessModeEnum.Always;

        _gameOverMenu = GetNode<Control>("GameOverMenu");
        _pauseMenu = GetNode<Control>("PauseMenu");
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _highScoreLabel = GetNode<Label>("HighScoreLabel");
        _startMenu = GetNode<Control>("StartMenu");

        Button startButton = _startMenu.GetNode<Button>("start");
        startButton.Pressed += _OnStartButtonPressed;

        Button resumeButton = _pauseMenu.GetNode<Button>("resume");
        resumeButton.Pressed += _OnResumeButtonPressed;

        Button restartButton = _gameOverMenu.GetNode<Button>("restart");
        restartButton.Pressed += _OnRestartButtonPressed;

        _gameOverMenu.Visible = false;
        _pauseMenu.Visible = false;
        _scoreLabel.Visible = false;
        _highScoreLabel.Visible = false;
        _startMenu.Visible = true;

        GetTree().Paused = true; // Pause the game at the start
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed && !eventKey.Echo)
        {
            if (eventKey.Keycode == Key.Escape)
            {
                if (_pauseMenu.Visible)
                {
                    _pauseMenu.Visible = false;
                    GetTree().Paused = false;
                }
                else
                {
                    ShowPauseMenu();
                }
            }
        }
    }

    public void _OnStartButtonPressed()
    {
        _startMenu.Visible = false;
        GetTree().Paused = false; // Unpause the game when starting
    }

    public void TogglePauseMenu()
    {
        if (_pauseMenu.Visible)
        {
            _pauseMenu.Visible = false;
            GetTree().Paused = false;
        }
        else
        {
            ShowPauseMenu();
        }
    }

    public void ShowPauseMenu()
    {
        _pauseMenu.Visible = true;
        GetTree().Paused = true;
    }

    public void _OnResumeButtonPressed()
    {
        TogglePauseMenu();
    }

    public void TriggerGameOver(int score, int highScore)
    {
        ShowGameOverMenu(score, highScore);
        GetTree().Paused = true; // Pause the game when showing the game over menu
    }

    public void ShowGameOverMenu(int score, int highScore)
    {
        _gameOverMenu.Visible = true;
        _scoreLabel.Text = $"Score: {score}";
        _highScoreLabel.Text = $"High Score: {highScore}";
        _scoreLabel.Visible = true;
        _highScoreLabel.Visible = true;
    }

    public void _OnRestartButtonPressed()
    {
        GetTree().Paused = false; // Unpause the game before restarting
        GetTree().ReloadCurrentScene();
    }

}
