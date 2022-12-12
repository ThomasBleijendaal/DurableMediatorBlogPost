using System.Collections.Generic;

namespace Fun.Models;

public record Order(string Id, List<Product> Products);
