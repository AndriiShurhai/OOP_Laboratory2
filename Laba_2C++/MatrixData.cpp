#include "MatrixData.h" 
#include <stdexcept>    
#include <sstream>      
#include <cmath>        
#include <iostream>     

MyMatrix::MyMatrix(const MyMatrix& other) : matrix(other.matrix) {}

MyMatrix::MyMatrix(const std::vector<std::vector<double>>& array) {
    validateJaggedMatrix(array);
    matrix = array;
}

MyMatrix::MyMatrix(const std::vector<std::string>& rows) {
    if (rows.empty()) {
        throw std::invalid_argument("Array cannot be empty");
    }

    std::istringstream firstRowStream(rows[0]);
    std::vector<double> firstRow;
    double num;

    while (firstRowStream >> num) {
        firstRow.push_back(num);
    }

    matrix.resize(rows.size(), std::vector<double>(firstRow.size()));

    for (size_t i = 0; i < rows.size(); ++i) {
        std::istringstream rowStream(rows[i]);
        size_t j = 0;

        while (rowStream >> num) {
            if (j >= firstRow.size()) {
                throw std::invalid_argument("All rows must have the same size");
            }
            matrix[i][j++] = num;
        }

        if (j != firstRow.size()) {
            throw std::invalid_argument("All rows must have the same number of elements");
        }
        if (matrix[i].empty()){
            throw std::invalid_argument("Matrix is empty, check if you gave everything correct loser(joke)");
        }
    }
    if (matrix.empty()){
        throw std::invalid_argument("Matrix is empty, check if you gave everything correct loser(joke)");
    }
}

MyMatrix::MyMatrix(const std::string& matrixString) {
    std::istringstream stream(matrixString);
    std::string line;
    std::vector<std::vector<double>> tempMatrix;

    while (std::getline(stream, line)) {
        std::istringstream lineStream(line);
        std::vector<double> row;
        double num;

        while (lineStream >> num) {
            row.push_back(num);
        }

        if (!tempMatrix.empty() && row.size() != tempMatrix[0].size() || row.empty()) {
            throw std::invalid_argument("All rows must have the same number of elements. Check if you gave informations correctly loser(joke)");
        }

        tempMatrix.push_back(row);
    }

    if (tempMatrix.empty()) {
        throw std::invalid_argument("String cannot be empty");
    }

    matrix = tempMatrix;
}

MyMatrix::~MyMatrix() {
    delete cachedDeterminant;
}

int MyMatrix::getHeight() const {
    return matrix.size();
}

int MyMatrix::getWidth() const {
    return matrix.empty() ? 0 : matrix[0].size();
}

double MyMatrix::getElement(int i, int j) const {
    validateIndexes(i, j);
    return matrix[i][j];
}

void MyMatrix::setElement(int i, int j, double value) {
    validateIndexes(i, j);
    matrix[i][j] = value;
    cachedDeterminant = nullptr;
}

double& MyMatrix::operator()(int i, int j) {
    validateIndexes(i, j);
    cachedDeterminant = nullptr;
    return matrix[i][j];
}

const double& MyMatrix::operator()(int i, int j) const {
    validateIndexes(i, j);
    return matrix[i][j];
}

std::string MyMatrix::toString() const {
    std::ostringstream result;
    for (size_t i = 0; i < matrix.size(); ++i) {
        for (size_t j = 0; j < matrix[i].size(); ++j) {
            result << matrix[i][j];
            if (j < matrix[i].size() - 1) {
                result << "\t";
            }
        }
        if (i < matrix.size() - 1) {
            result << "\n";
        }
    }
    return result.str();
}

void MyMatrix::validateIndexes(int i, int j) const {
    if (i < 0 || i >= getHeight()) {
        throw std::out_of_range("Row index " + std::to_string(i) + " is out of range [0, " + std::to_string(getHeight()) + ")");
    }
    if (j < 0 || j >= getWidth()) {
        throw std::out_of_range("Column index " + std::to_string(j) + " is out of range [0, " + std::to_string(getWidth()) + ")");
    }
}

void MyMatrix::validateSquareSize() const{
    if(getHeight() != getWidth()){
        throw std::invalid_argument("Matrix must be square");
    }
}

void MyMatrix::validateJaggedMatrix(std::vector<std::vector<double>> array) const{
    if (array.empty() || array[0].size() == 0) {
        throw std::invalid_argument("Matrix cannot be empty");
    }

    int width = array[0].size();

    for (int i = 1; i < array.size(); i++)
        if (array[i].size() != width)
            throw std::invalid_argument("All rows in matrix must be the same size");
}