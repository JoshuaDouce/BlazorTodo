using System;
using Xunit;
using Bunit;
using Bunit.TestDoubles.JSInterop;
using static Bunit.ComponentParameterFactory;
using BlazorTodo.Client.Pages;
using AngleSharp.Dom;

namespace BlazorTodo.Tests
{
    /// <summary>
    /// These tests are written entirely in C#.
    /// Learn more at https://bunit.egilhansen.com/docs/
    /// </summary>
    public class TodoComponentCSharpTests : TestContext
    {
        [Fact]
        public void TodoCountStartsAtZero()
        {
            // Arrange
            var cut = RenderComponent<Todo>();

            // Assert
            Assert.Contains("Todo (0)", cut.Markup);
        }

        [Fact]
        public void TodoComponentRendersInDefaultState()
        {
            // Arrange
            var cut = RenderComponent<Todo>();

            // Assert
            cut.MarkupMatches(@"<h3 diff:ignore></h3>
                                <ul diff:ignore></ul>
                                <input diff:ignore/>
                                <button diff:ignore></button>");
            Assert.DoesNotContain("<li></li>", cut.Markup);
        }

        [Fact]
        public void TodoAddButtonClickedTodoIsAdded()
        {
            // Arrange
            var cut = RenderComponent<Todo>();
            cut.Find("input").Change("New to do");
            
            //Act
            cut.Find("button").Click();

            // Assert
            cut.Find("li").MarkupMatches(@"<li>" +
                                         "<input diff:ignore/>" +
                                         "<input value=\"New to do\">" +
                                         "</li>");
            Assert.Contains("Todo (1)", cut.Markup);
        }

        [Fact]
        public void TodoAddButtonClickedWIthNoInputTodoIsNotAdded()
        {
            // Arrange
            var cut = RenderComponent<Todo>();
            
            //Act
            cut.Find("button").Click();

            // Assert
            cut.MarkupMatches(@"<h3>Todo (0)</h3>
                                <ul diff:ignore></ul>
                                <input diff:ignore/>
                                <button diff:ignore></button>");
            Assert.DoesNotContain("<li></li>", cut.Markup);
        }

        [Fact]
        public void TodoCheckToDoReducesCount()
        {
            // Arrange
            var cut = RenderComponent<Todo>();
            cut.Find("input").Change("New to do");
            cut.Find("button").Click();
            
            //Act
            cut.Find("li").FirstElementChild.Change(true);

            // Assert
            Assert.Contains("Todo (0)", cut.Markup);
        }

        [Fact]
        public void TodoUnCheckToDoIncreasesCount()
        {
            // Arrange
            var cut = RenderComponent<Todo>();
            cut.Find("input").Change("New to do");
            cut.Find("button").Click();
            cut.Find("li").FirstElementChild.Change(true);
            
            //Act
            cut.Find("li").FirstElementChild.Change(false);

            // Assert
            Assert.Contains("Todo (1)", cut.Markup);
        }
    }
}
