import numpy as np

class MyMatrix:
    def __init__(self, source):
        self.matrix = None
        self._cached_determinant = None

        if isinstance(source, MyMatrix):
            self.matrix = np.array(source.matrix, copy=True)

        elif isinstance(source, np.ndarray):
            self.matrix = source.copy()

        elif isinstance(source, list):
            if all(isinstance(row, list) for row in source):
                self._validate_jagged_array(source)
                self.matrix = np.array(source, dtype=float)

            elif all(isinstance(row, str) for row in source):
                self.matrix = self._from_string_list(source)

            else:
                raise ValueError("Invalid source data for matrix")

        elif isinstance(source, str):
            self.matrix = self._from_string(source)

        else:
            raise ValueError("Unsupported type for matrix initialization")

    @property
    def height(self):
        return self.matrix.shape[0]

    @property
    def width(self):
        return self.matrix.shape[1]
    
    def get_height(self):
        return self.matrix.shape[0]
    
    def get_width(self):
        return self.matrix.shape[1]

    def __getitem__(self, indices):
        i, j = indices
        self._validate_indexes(i, j)
        return self.matrix[i, j]

    def __setitem__(self, indices, value):
        i, j = indices
        self._validate_indexes(i, j)
        self._cached_determinant = None
        self.matrix[i, j] = value

    def get_element(self, i, j):
        self._validate_indexes(i, j)
        return self.matrix[i, j]

    def set_element(self, i, j, value):
        self._validate_indexes(i, j)
        self._cached_determinant = None
        self.matrix[i, j] = value

    def _validate_indexes(self, i, j):
        if not (0 <= i < self.height):
            raise IndexError(f"Row index {i} is out of range [0, {self.height})")
        if not (0 <= j < self.width):
            raise IndexError(f"Column index {j} is out of range [0, {self.width})")

    def _validate_jagged_array(self, jagged_array):
        if not jagged_array or any(len(row) != len(jagged_array[0]) for row in jagged_array):
            raise ValueError("Matrix must be rectangular")

    def _from_string_list(self, rows):
        if not rows:
            raise ValueError("Array cannot be empty")
        matrix = []
        for i, row in enumerate(rows):
            try:
                numbers = list(map(float, row.split()))
                if len(matrix) > 0 and len(numbers) != len(matrix[0]):
                    raise ValueError("All rows must have the same number of elements")
                matrix.append(numbers)
            except ValueError:
                raise ValueError(f"Invalid number format at row {i}")
        return np.array(matrix)

    def _from_string(self, matrix_string):
        rows = matrix_string.strip().splitlines()
        return self._from_string_list(rows)

    def __str__(self):
        return "\n".join("\t".join(map(str, row)) for row in self.matrix)
