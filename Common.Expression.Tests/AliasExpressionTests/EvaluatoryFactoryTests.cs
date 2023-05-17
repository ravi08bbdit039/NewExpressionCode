using Common.Expression.AliasExpressions.Evaluators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Expressions.AliasExpressionTests
{
    public class EvaluatorFactoryTests: TestBase
    {
        private IEvaluatorFactory _evaluatorFactory;
        
        [TestInitialize]
        public new void Init ()
        {
            base.Init();
            _evaluatorFactory = new EvaluatorFactory();
        }

        [TestMethod]
        public void NumericExpression_AsString_IsProperlyCalculated_UsingEvaluatoryFactory()
        {
            var evaluator = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), _textExpression);
            var result = evaluator.Evaluate(_provider);

            Assert.IsTrue(Math.Abs(result - 116.2762) <= _delta);     
        }

        [TestMethod]
        public void NumericExpression_AsExpression_IsProperlyCalculated_UsingEvaluatoryFactory()
        {
            var expression = _parser.ToExpression(_textExpression);
            var evaluator = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), expression);
            var result = evaluator.Evaluate(_provider);

            Assert.IsTrue(Math.Abs(result - 116.2762) <= _delta);     
        }

        [TestMethod]
        public void Factory_CreatesAsEvaluator_OnceOnly_ForTheSameInput1()
        {
            var evaluator1 = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), _textExpression);
            var evaluator2 = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), _textExpression);
            Assert.IsTrue(ReferenceEquals(evaluator1, evaluator2));
        }

        [TestMethod]
        public void Factory_CreatesAsEvaluator_OnceOnly_ForTheSameInput2()
        {
            var expression = _parser.ToExpression(_textExpression);
            var evaluator1 = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), expression);
            var evaluator2 = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), expression);

            Assert.IsTrue(ReferenceEquals(evaluator1, evaluator2));
        }

        [TestMethod]
        public void Factory_CreatesDifferentEvaluators_When_DiffentInputsProvided()
        {
            var evaluator1 = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), _textExpression);
            var evaluator2 = (IEvaluator<string, double>) _evaluatorFactory.GetEvaluator(typeof(string), _textExpression);

            Assert.IsFalse(ReferenceEquals(evaluator1, evaluator2));
        }
    }
}