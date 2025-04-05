"use strict";
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
    result["default"] = mod;
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
const react_1 = __importStar(require("react"));
const ProductSearch = () => {
    const [products, setProducts] = react_1.useState([]);
    const [filtered, setFiltered] = react_1.useState([]);
    const [query, setQuery] = react_1.useState("");
    react_1.useEffect(() => {
        fetch('/Product/GetAll') // Adjust if your API path is different
            .then(res => res.json())
            .then(data => {
            setProducts(data);
            setFiltered(data);
        })
            .catch(err => console.error("Failed to load products", err));
    }, []);
    react_1.useEffect(() => {
        setFiltered(products.filter(p => p.name.toLowerCase().includes(query.toLowerCase())));
    }, [query, products]);
    return (react_1.default.createElement("div", { className: "container mt-3" },
        react_1.default.createElement("input", { type: "text", className: "form-control mb-3", placeholder: "Search products by name...", value: query, onChange: e => setQuery(e.target.value) }),
        react_1.default.createElement("table", { className: "table table-bordered table-striped" },
            react_1.default.createElement("thead", { className: "table-dark" },
                react_1.default.createElement("tr", null,
                    react_1.default.createElement("th", null, "Name"),
                    react_1.default.createElement("th", null, "Price"),
                    react_1.default.createElement("th", null, "Quantity"))),
            react_1.default.createElement("tbody", null, filtered.map(product => (react_1.default.createElement("tr", { key: product.id },
                react_1.default.createElement("td", null, product.name),
                react_1.default.createElement("td", null,
                    "$",
                    product.price.toFixed(2)),
                react_1.default.createElement("td", null, product.quantity))))))));
};
exports.default = ProductSearch;
