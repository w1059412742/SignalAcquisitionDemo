using System.ComponentModel;

public class ChannelPropertyData : INotifyPropertyChanged
{
    private float _Value;

    public float Value
    {
        get { return _Value; }
        set
        {
            if (_Value != value)
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class SwitchPropertyData : INotifyPropertyChanged
{
    private bool _Value;

    public bool Value
    {
        get { return _Value; }
        set
        {
            if (_Value != value)
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
