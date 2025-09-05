import { Suspense } from 'react'
import { getQuizzes, getCategories } from '@/lib/cosmic'
import QuizCard from '@/components/QuizCard'
import CategoryFilter from '@/components/CategoryFilter'
import Loading from '@/app/loading'

interface PageProps {
  searchParams: Promise<{ category?: string }>
}

export default async function QuizzesPage({ searchParams }: PageProps) {
  const params = await searchParams
  const selectedCategory = params.category || null
  
  const [quizzes, categories] = await Promise.all([
    getQuizzes(selectedCategory || undefined),
    getCategories()
  ])

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-4xl font-bold mb-8">All Quizzes</h1>
      
      <div className="mb-8">
        <CategoryFilter 
          categories={categories} 
          selectedCategory={selectedCategory}
        />
      </div>

      <Suspense fallback={<Loading />}>
        {quizzes.length > 0 ? (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {quizzes.map((quiz) => (
              <QuizCard key={quiz.id} quiz={quiz} />
            ))}
          </div>
        ) : (
          <div className="text-center py-12">
            <p className="text-gray-600 text-lg">
              No quizzes available in this category. Try selecting a different category or check back later!
            </p>
          </div>
        )}
      </Suspense>
    </div>
  )
}