using System;
using System.Linq;

public class Class1
{
	public Class1()
	{
		Good iPhone12 = new Good("IPhone 12");
		Good iPhone11 = new Good("IPhone 11");

		Warehouse warehouse = new Warehouse();

		Shop shop = new Shop(warehouse);

		warehouse.Delive(iPhone12, 10);
		warehouse.Delive(iPhone11, 1);

		//Вывод всех товаров на складе с их остатком

		Cart cart = shop.Cart();
		cart.Add(iPhone12, 4);
		cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

		//Вывод всех товаров в корзине

		Console.WriteLine(cart.Order().Paylink);

		cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
	}
}

class Good
{
	public string Name { get; private set; }

	public Good(string name)
    {
		Name = name;
    }
}

class Shop
{
	private Warehouse _warehouse;

	public Shop(Warehouse warehouse)
    {
		_warehouse = warehouse;
    }

	public Cart Cart()
    {
		return new Cart(_warehouse);
    }
}

class Warehouse
{
	private List<GoodCell> _goodsCells;

	public IReadOnlyList<GoodCell> GoodCells => _goodsCells;

	public void Delive(Good good, int count)
    {
		if (count <= 0)
			throw new ArgumentOutOfRangeException();

		GoodCell cell = _goodsCells.FirstOfDefault(cell => cell.Name == good.Name);

		if (cell != null)
			cell = cell.Merge(new GoodCell(good, count));
		else
			_goodsCells.Add(new GoodCell(good, count));
    }

	public void RemoveGoods(IReadOnlyList<GoodCell> cellToBeRemoved)
    {
		foreach (var goodCell in cellToBeRemoved)
		{
			GoodCell cell = _goodsCells.FirstOfDefault(cell => cell.Name == goodCell.Name);

			if (cell != null)
			{
				if (_goodsCells[cell].Count >= cellToBeRemoved[cell])
				{
					if (_goodsCells[cell].Count - cellToBeRemoved[cell] == 0)
						_goodsCells.Remove(_goodsCells[cell]);
					else
						_goodsCells[cell] = new GoodCell(cell.Name, _goodsCells[cell].Count - cell.Count);
				}
				else
					throw new InvalidOperationException();
			}
            else
            {
				throw new InvalidOperationException();
            }
		}
    }

	public void ShowAllGoods()
    {
		foreach(var goodCell in _goodsCells)
        {
			Console.WriteLine($"{goodCell.Name} : {goodCell.Count}");
        }
    }
}

class Cart
{
	private Warehouse _shopWarehouse;
	private List<GoodCell> _goodsCells;

	public IReadOnlyList<GoodCell> GoodsCells => _goodsCells;

	public Cart(Warehouse warehouse)
    {
		_shopWarehouse = warehouse;
    }

	public void Add(Good good, int count)
	{
		GoodCell cell = _shopWarehouse.GoodCells.FirstOfDefault(cell => cell.Name == good.Name);

		if (cell != null)
		{
			if (cell.Count >= count)
			{
				GoodCell cartCell = _goodsCells.FirstOfDefault(cell => cell.Name == good.Name);

				if (cartCell != null)
					cartCell = cartCell.Merge(new GoodCell(good, count));
				else
					_goodsCells.Add(new GoodCell(good, count));
			}
			else
				throw new InvalidOperationException();
		}
	}

	public string Order()
    {		
		_shopWarehouse.RemoveGoods(GoodsCells);

		_goodsCells = new List<GoodCell>();

		return "@134123&123&8899";
    }

	public void ShowGoods()
	{
		foreach (var goodCell in _goodsCells)
		{
			Console.WriteLine($"{goodCell.Name} : {goodCell.Count}");
		}
	}
}

class GoodCell
{
	public int Count { get; private set; }
	public string Name => _good.Name;
	
	private Good _good;

	public GoodCell(Good good, int count)
    {
		_good = good;
		Count = count;
    }

	public GoodCell Merge(GoodCell cell)
    {
		return new GoodCell(_good, Count + cell.Count);
    }
}