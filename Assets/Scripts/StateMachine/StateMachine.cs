using System;
using System.Collections.Generic;

/**
 * From Jason Weimann
 * https://game.courses/bots-ai-statemachines/
 */

public class StateMachine
{
   private IState currentState;
   
   private Dictionary<Type, List<Transition>> allTransitions = new Dictionary<Type,List<Transition>>();
   private List<Transition> currentTransitions = new List<Transition>();
   private List<Transition> anyTransitions = new List<Transition>();
   
   private static List<Transition> EmptyTransitions = new List<Transition>(0);

   private class Transition
   {
      public Func<bool> Condition {get; }
      public IState To { get; }

      public Transition(IState to, Func<bool> condition)
      {
         To = to;
         Condition = condition;
      }
   }

   public void Update()
   {
      var transition = GetTransition();
      if (transition != null)
         SetState(transition.To);
      
      currentState?.Update();
   }

   public void SetState(IState state)
   {
      if (state == currentState)
         return;
      
      currentState?.OnExit();
      currentState = state;
      
      allTransitions.TryGetValue(currentState.GetType(), out currentTransitions);
      if (currentTransitions == null)
         currentTransitions = EmptyTransitions;
      
      currentState.OnEnter();
   }

   public void AddTransition(IState from, IState to, Func<bool> predicate)
   {
      if (allTransitions.TryGetValue(from.GetType(), out var transitions) == false)
      {
         transitions = new List<Transition>();
         allTransitions[from.GetType()] = transitions;
      }
      
      transitions.Add(new Transition(to, predicate));
   }

   public void AddAnyTransition(IState state, Func<bool> predicate)
   {
      anyTransitions.Add(new Transition(state, predicate));
   }

   private Transition GetTransition()
   {
      foreach(var transition in anyTransitions)
         if (transition.Condition())
            return transition;
      
      foreach (var transition in currentTransitions)
         if (transition.Condition())
            return transition;

      return null;
   }
}