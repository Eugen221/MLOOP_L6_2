using System;
using System.Collections.Generic;
using System.Linq;

// Базовий клас Документ
public abstract class Document
{
    public string DocId { get; }
    public string Date { get; }

    protected Document(string docId, string date)
    {
        DocId = docId;
        Date = date;
    }

    public abstract double GetTotalAmount();

    public override string ToString()
    {
        return $"Document ID: {DocId}, Date: {Date}";
    }
}

// Клас Квитанція
public class Receipt : Document
{
    public double Amount { get; }

    public Receipt(string docId, string date, double amount)
        : base(docId, date)
    {
        Amount = amount;
    }

    public override double GetTotalAmount()
    {
        return Amount;
    }

    public override string ToString()
    {
        return $"Receipt [{base.ToString()}, Amount: {Amount}]";
    }
}

// Клас Чек
public class Check : Document
{
    public List<Item> Items { get; }

    public Check(string docId, string date, List<Item> items)
        : base(docId, date)
    {
        Items = items;
    }

    public override double GetTotalAmount()
    {
        return Items.Sum(item => item.Price * item.Quantity);
    }

    public override string ToString()
    {
        return $"Check [{base.ToString()}, Total Amount: {GetTotalAmount()}]";
    }
}

// Клас Накладна
public class Invoice : Document
{
    public List<Item> Items { get; }

    public Invoice(string docId, string date, List<Item> items)
        : base(docId, date)
    {
        Items = items;
    }

    public override double GetTotalAmount()
    {
        return Items.Sum(item => item.Price * item.Quantity);
    }

    public double GetItemAmount(string itemName)
    {
        return Items.Where(item => item.Name == itemName)
                   .Sum(item => item.Price * item.Quantity);
    }

    public override string ToString()
    {
        return $"Invoice [{base.ToString()}, Total Amount: {GetTotalAmount()}]";
    }
}

// Клас для елемента (продукту)
public class Item
{
    public string Name { get; }
    public double Price { get; }
    public double Quantity { get; }

    public Item(string name, double price, double quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}

// Статичний клас для обчислень
public static class InvoiceCalculator
{
    public static double TotalItemAmountInInvoices(List<Invoice> invoices, string itemName)
    {
        return invoices.Sum(invoice => invoice.GetItemAmount(itemName));
    }
}

// Приклад використання
class Program
{
    static void Main()
    {
        // Створюємо кілька накладних
        var invoices = new List<Invoice>
        {
            new Invoice("INV001", "2025-06-01", new List<Item>
            {
                new Item("Apple", 2.5, 10),
                new Item("Banana", 1.5, 20)
            }),
            new Invoice("INV002", "2025-06-02", new List<Item>
            {
                new Item("Apple", 2.7, 15),
                new Item("Orange", 3.0, 5)
            })
        };

        // Обчислюємо сумарну вартість продукту "Apple"
        string itemName = "Apple";
        double total = InvoiceCalculator.TotalItemAmountInInvoices(invoices, itemName);
        Console.WriteLine($"Сумарна вартість продукту '{itemName}' за всіма накладними: {total}");

        // Виведення інформації про документи
        foreach (var invoice in invoices)
        {
            Console.WriteLine(invoice);
        }
    }
}