<template>
    <div>
        <canvas id="myChart" width="160" height="160"></canvas>
    </div>    
</template>
<script>
import Chart from 'chart.js';

export default {
    name: "HomeChart",
    props: {
        hourData: {
            type: Array,
            default: () => []
        },
    },
    data() {
        return {
            MyData: {}
        }
    },
    mounted () {
        var ctx = document.getElementById("myChart");
        new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ['Taken', 'Planned', 'Available'],
                datasets: [{
                label: 'Hours Summary',
                data: this.hourData || [36,56,8],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.5)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)'
                ],
                borderWidth: 1
                }]
            },
            options: {
                circumference: Math.PI,
                rotation: -Math.PI,
                cutoutPercentage: 40,
                responsive: false,

            }
        });
    }
}
</script>

<style scoped>
</style>