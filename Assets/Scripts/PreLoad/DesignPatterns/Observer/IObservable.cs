using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IObservable<TValue>
{
    void Subscribe(IObserver<TValue> pObserver);
    void UnSubscribe(IObserver<TValue> pObserver);
    void SendNotify(TValue pValue);
}