! Computes the average and standard deviation of 200 numbers.
    
      program stats
            
      integer n
      parameter (n = 200)
      integer info(n)
      real average, avg, stddev, sd
      common /precomputed/ avg
      
      data info/  191,   290,  6336,  9793,  3803,  3182,  4584,  8726/
!     +           8573,  2115,  3475,  5299,  7781,  2438,   902,  3293,
!     +           1030,   240,   491,  6732,  7208,  7236,  9335,   664,
!     +            484,  6333,  1638,  4971,  4494,  2963,   232,  5449,
!     +           7162,  4017,  7236,  5060,  5468,  6690,  8675,  8466,
!     +           5134,  2270,  4657,  6342,  1545,  5498,  5727,  9950,
!     +           6904,  3958,  2633,  5840,  5576,  1928,  2970,  4146,
!     +           7816,  7792,  2655,  3679,  8455,  8516,  1523,  8232,
!     +           2569,  1106,  6169,  3836,  5567,   947,   693,  9896,
!     +           3131,  5483,  9764,  1079,  5128,    50,  8784,  8894,
!     +           5967,  4362,  4051,  5822,   887,  2102,  5193,  6361,
!     +           1609,   261,  7894,  6445,  6446,  7993,  2127,  8981,
!     +           5377,  6314,  1679,  1985,  9796,   701,  9957,   170,
!     +           9719,  6934,  3400,  6169,  2864,  8306,  1928,  9002,
!     +           4326,  5536,  8391,  4908,  7127,  8181,  3534,  6745,
!     +           9643,  1459,  4168,  8023,  6017,    90,    92,  8395,
!     +           7099,  1179,  3067,   752,  7139,  8655,  8807,  6097,
!     +           8973,  3869,  6805,  2873,  5950,  7888,  2865,   781,
!     +             56,  7596,  4503,  2710,  8421,  4920,  2517,  5603,
!     +           3167,  5914,  9909,  5376,  1232,  8654,  1106,  5139,
!     +           2900,  7200,  9526,  5810,  6654,  8807,  3143,  4006,
!     +           6858,  4318,  4507,  6298,  8915,  7407,  2960,  5792,
!     +           1317,  2034,  9695,  9300,  1924,  4515,  6363,  5177,
!     +           3964,  1363,  2954,   861,  6918,  8342,  3967,  7787,
!     +           7862,  6986,  8056,  8538,  7123,   910,  6956,  2731/
     
      avg = average(info)      
      sd = stddev(info)
      write(*, *) 'Average =', avg      
      write(*, *) 'Standard Deviation =', sd
      
      stop
      end
      
!-------------------------------------------------------------------------------
! Compute the average of info.
    
      real function average(info)
      
      parameter (n = 200)
      integer info(n)      
      integer i
      real sum
      
      sum = 0
      do 10 i = 1, n
          sum = sum + info(i)
10    continue

      average = sum / n
      return
      end
      
!-------------------------------------------------------------------------------
! Compute the standard deviation of info.
    
      real function stddev(info)
      
      parameter (n = 200)
      integer info(n)
      integer i
      real sum, avg
      common /precomputed/ avg
      
      sum = 0
      do 10 i = 1, n
          sum = sum + ((info(i) - avg) ** 2)          
10    continue

      stddev = sqrt(sum / n)
      return
      end      
      