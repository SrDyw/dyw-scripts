using System.Collections.Generic;
using UnityEngine;

public abstract class Subject:MonoBehaviour {
  private List<IObserver> _observers = new List<IObserver>();
  void Subscribe(IObserver ob) {
    _observers.Add(ob);
  }
  void Unsubscribe(IObserver ob) {
     _observers.Remove(ob);
  }
  void Notify() {
    _observers.ForEach(ob => ob.OnNotify());
  }
}
