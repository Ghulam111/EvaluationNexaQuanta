

import React from 'react';
import { createRoot } from 'react-dom/client';
import ProductSearch from './ProductSearch';

const container = document.getElementById('react-product-search');
const root = createRoot(container!);
root.render(<ProductSearch />);
