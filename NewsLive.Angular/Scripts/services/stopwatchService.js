(function (angular) {
    angular.module('app')
        .service('$stopwatchService', ['$interval', function ($interval) {
            var scope = this;
            var stopwatch = undefined;
            var started = 0, stopped = 0, current = 0, ended = 0, lapsed = 0;
            var now = function () {
                return (new Date()).getTime();
            };
            var update = function () {
                current = now();
            };
            scope.start = function () {
                if (angular.isDefined(stopwatch)) return;
                started = now();
                stopwatch = $interval(function () {
                    update();
                }, 1);
            };
            scope.pause = function () {
                if (angular.isDefined(stopwatch)) {
                    $interval.cancel(stopwatch);
                    stopwatch = undefined;
                }
            };
            scope.stop = function () {
                lapsed = current - started;
                stopped = now();
                current = 0;
                ended = 0;
                stopwatch = undefined;
                return lapsed;
            };
            scope.reset = function () {
                stopped = 0;
                started = 0;
                current = 0;
                ended = 0;
                lapsed = 0;
            };
            scope.lapsedMilliseconds = function () {
                var milliseconds = lapsed;
                lapsed = 0
                return milliseconds;
            };
            scope.lapsedFormatted = function (context) {
                return context + ' took ' + scope.lapsedMilliseconds() + ' milliseconds';
            };
        }]);
})(angular);


