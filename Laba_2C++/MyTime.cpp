#include<iostream>
#include<cmath>
#include<iomanip>
#include<sstream>
#include<vector>
#include<string>

class MyTime{
    private:
        int hour;
        int minute;
        int second;

        void setFromSeconds(int totalSeconds){
            int secPerDay = 60 * 60 * 24;
            totalSeconds %= secPerDay;

            if (totalSeconds < 0){
                totalSeconds += secPerDay;
            }

            hour = totalSeconds / 3600;
            minute = (totalSeconds / 60) % 60;
            second = totalSeconds % 60;
        }

        public:
            MyTime() {
                hour = 0;
                minute = 0;
                second = 0;
            }
            MyTime(int h, int m, int s){
                int totalSeconds = (h * 3600) + (m*60) + s;
                setFromSeconds(totalSeconds);
            }

            int getHour() const{return hour;}
            int getMinute() const{return minute;}
            int getSecond() const{return second;}

            std::string toString() const{
                std::ostringstream oss;
                oss << std::setfill('0')
                    << std::setw(2) << hour << ":"
                    << std::setw(2) << minute << ":"
                    << std::setw(2) << second;
                return oss.str();
            }

            int timeSinceMidnight() const{
                return hour * 3600 + minute * 60 + second;
            }

            void addSeconds(int seconds){
                int totalSeconds = timeSinceMidnight() + seconds;
                setFromSeconds(totalSeconds);
            }

            void addOneSecond(){
                setFromSeconds(timeSinceMidnight() + 1);
            }

            void addOneMinute(){
                setFromSeconds(timeSinceMidnight() + 60);
            }

            void addOneHour(){
                setFromSeconds(timeSinceMidnight() + 3600);
            }

            int difference(const MyTime& other) const{
                return timeSinceMidnight() - other.timeSinceMidnight();
            }

            std::string whatLesson() const {
                int timeInSeconds = timeSinceMidnight();
                
                std::vector<int> startTimes = {
                    8 * 3600,              
                    9 * 3600 + 20 * 60,    
                    9 * 3600 + 40 * 60,    
                    11 * 3600,             
                    11 * 3600 + 20 * 60,   
                    12 * 3600 + 40 * 60,    
                    13 * 3600,              
                    14 * 3600 + 20 * 60,   
                    14 * 3600 + 40 * 60,    
                    16 * 3600,             
                    16 * 3600 + 10 * 60,    
                    17 * 3600 + 30 * 60,   
                    17 * 3600 + 40 * 60,   
                    19 * 3600         
                };

                std::vector<std::string> lessons = {
                    "First lecture", "break between 1 and 2 lectures",
                    "Second lecture", "break between 1 and 2 lectures",
                    "Third lecture", "break between 1 and 2 lectures",
                    "Fourth lecture", "break between 1 and 2 lectures", 
                    "Fifth lecture", "break between 1 and 2 lectures",
                    "Sixth lecture", "break between 1 and 2 lectures", 
                    "Seventh lecture", "lectures are over"
                };

                for (size_t i = 0; i < startTimes.size() - 1; i += 2) {
                    if (timeInSeconds < startTimes[i]) {
                        return i == 0 ? "lectures hasn't started yet" : lessons[i - 1];
                    }
                    else if (timeInSeconds < startTimes[i + 1]) {
                        return lessons[i];
                    }
                }

                return "lectures are over";
            }
};