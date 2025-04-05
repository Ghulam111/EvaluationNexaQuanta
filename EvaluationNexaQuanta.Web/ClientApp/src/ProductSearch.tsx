import React, { useState, useEffect } from 'react';

interface Product {
    id: number;
    name: string;
    price: number;
}

const ProductSearch: React.FC = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [products, setProducts] = useState<Product[]>([]);
    const [filteredProducts, setFilteredProducts] = useState<Product[]>([]);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await fetch('/Product/GetAll');
                const data: Product[] = await response.json();
                setProducts(data);
                setFilteredProducts([]); // Initially no products shown
            } catch (error) {
                console.error('Error fetching products:', error);
            }
        };

        fetchProducts();
    }, []); // Fetch all products once on component mount

    useEffect(() => {
        if (searchTerm === '') {
            setFilteredProducts([]); // Clear results when search is blank
        } else {
            const filtered = products.filter(product =>
                product.name.toLowerCase().includes(searchTerm.toLowerCase())
            );
            setFilteredProducts(filtered);
        }
    }, [searchTerm, products]); // Trigger search when searchTerm or products change

    return (
        <div>
            <div className="input-group mb-3">
                <input
                    type="text"
                    className="form-control"
                    placeholder="Search products..."
                    value={searchTerm}
                    onChange={e => setSearchTerm(e.target.value)}
                />
            </div>

            {filteredProducts.length > 0 && (
                <table className="table table-bordered">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredProducts.map(p => (
                            <tr key={p.id}>
                                <td>{p.name}</td>
                                <td>{p.price.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default ProductSearch;
