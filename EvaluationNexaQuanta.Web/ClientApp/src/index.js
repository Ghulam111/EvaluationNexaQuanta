"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const react_1 = __importDefault(require("react"));
const client_1 = require("react-dom/client");
const ProductSearch_1 = __importDefault(require("./ProductSearch"));
const container = document.getElementById('react-product-search');
const root = client_1.createRoot(container);
root.render(react_1.default.createElement(ProductSearch_1.default, null));
