#include "MyTime.cpp"
#include<iostream>


int main(void){
    MyTime time(100, 100, 100);

    std::cout<<time.toString()<<std::endl;
    std::cout<<time.whatLesson()<<std::endl;

    time = MyTime(-29, -19, -59);
    std::cout<<time.toString()<<std::endl;
    std::cout<<time.whatLesson()<<std::endl;

    time.addOneSecond();
    std::cout<<time.whatLesson()<<std::endl;
}